import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { catchError, Observable, retry, throwError, timeout } from 'rxjs';
import { Album } from '../models/album.model';
import { environment } from '../../environments/environment';

export interface SessionResponse {
    sessionId: string;
    albums: Album[];
    restartsRemaining: number;
}

@Injectable({
    providedIn: 'root'
})
export class RecommendationService {
    private readonly sessionUrl = `${environment.apiUrl}/sessions`;
    private readonly anonIdPattern = /^[a-zA-Z0-9_-]{16,128}$/;

    constructor(private http: HttpClient) { }

    // ── Anonymous ID ──────────────────────────────────────────────────────────

    /** Returns the persisted anonymous ID, creating and storing one on first call. */
    getOrCreateAnonId(): string {
        const key = 'music_anon_id';
        let id = localStorage.getItem(key);

        // Backend validates this header strictly. Regenerate if missing/legacy/invalid.
        if (!id || !this.anonIdPattern.test(id)) {
            id = crypto.randomUUID();
            localStorage.setItem(key, id);
        }
        return id;
    }

    // ── Session API ───────────────────────────────────────────────────────────

    createSession(
        energy: number,
        familiarity: number,
        time: number
    ): Observable<SessionResponse> {
        return this.withResilience(this.http.post<SessionResponse>(
            this.sessionUrl,
            { energy, familiarity, time },
            { headers: this.anonHeaders() }
        ));
    }

    restart(sessionId: string): Observable<SessionResponse> {
        return this.withResilience(this.http.post<SessionResponse>(
            `${this.sessionUrl}/${sessionId}/restart`,
            null,
            { headers: this.anonHeaders() }
        ));
    }

    // ── Private ───────────────────────────────────────────────────────────────

    private anonHeaders(): HttpHeaders {
        return new HttpHeaders({ 'X-Anonymous-Id': this.getOrCreateAnonId() });
    }

    private withResilience<T>(request$: Observable<T>): Observable<T> {
        return request$.pipe(
            timeout(10000),
            retry({ count: 1, delay: 400 }),
            catchError((error) => throwError(() => error))
        );
    }
}
