import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Album } from '../models/album.model';

@Injectable({
    providedIn: 'root'
})
export class RecommendationService {
    private apiUrl = '/api/recommendations'; // Backend URL encapsulated here

    constructor(private http: HttpClient) { }

    getRecommendedAlbums(
        energy: number,
        emotion: number,
        familiarity: number,
        time: number
    ): Observable<{ albums: Album[] }> {
        return this.http.post<{ albums: Album[] }>(this.apiUrl, {
            energy,
            emotion,
            familiarity,
            time
        });
    }
}
