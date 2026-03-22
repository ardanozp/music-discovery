import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpErrorResponse } from '@angular/common/http';
import { trigger, transition, style, animate } from '@angular/animations';
import { RecommendationService, SessionResponse } from '../services/recommendation.service';
import { Album } from '../models/album.model';
import { generateScore } from '../utils/score-mapper.util';
import { environment } from '../../environments/environment';

type Step = 'energy' | 'familiarity' | 'time' | 'loading' | 'album' | 'limited' | 'dailyLocked';
type ErrorState = 'network' | 'server' | 'unknown';

@Component({
    selector: 'app-album-screen',
    imports: [CommonModule],
    templateUrl: './album-screen.component.html',
    styleUrl: './album-screen.component.css',
    animations: [
        trigger('albumTransition', [
            transition('* => *', [
                style({ opacity: 0, transform: 'translateX(30px)' }),
                animate('500ms ease-out', style({ opacity: 1, transform: 'translateX(0)' }))
            ])
        ]),
        trigger('fadeTransition', [
            transition('* => *', [
                style({ opacity: 0 }),
                animate('800ms 300ms ease-out', style({ opacity: 1 }))
            ])
        ])
    ]
})
export class AlbumScreenComponent implements OnInit {
    energyScore = 0;
    familiarityScore = 0;
    timeScore = 0;

    // ── Session state ─────────────────────────────────────────────────────────
    sessionId: string | null = null;
    restartsRemaining = 0;

    // ── UI state ──────────────────────────────────────────────────────────────
    albums: Album[] = [];
    seenAlbums: Album[] = [];
    dailyLockedAlbums: Album[] = [];
    currentStep: Step = 'energy';
    currentAlbumIndex = 0;
    isLoadingNext = false;
    errorMessage = '';

    constructor(private recommendationService: RecommendationService) { }

    ngOnInit(): void {
        if (environment.bypassDailyLimit) {
            // Dev mode: clear any locked daily-limit state so testing is unrestricted.
            this.clearAllDailyLimitKeys();
            return;
        }

        const saved = this.getDailyLimitState();
        if (saved?.locked && saved.albums?.length) {
            this.dailyLockedAlbums = saved.albums;
            this.currentStep = 'dailyLocked';
        }
    }

    // ── Helpers ───────────────────────────────────────────────────────────────

    get currentAlbum(): Album | null {
        return this.albums.length ? this.albums[this.currentAlbumIndex] : null;
    }

    get spotifyUrl(): string {
        if (!this.currentAlbum) return '';
        const query = encodeURIComponent(`${this.currentAlbum.artist} ${this.currentAlbum.title}`);
        return `https://open.spotify.com/search/${query}/albums`;
    }

    // ── Quiz actions ──────────────────────────────────────────────────────────

    setEnergy(energy: string) {
        this.energyScore = generateScore('energy', energy);
        this.currentStep = 'familiarity';
    }

    setFamiliarity(familiarity: string) {
        this.familiarityScore = generateScore('familiarity', familiarity);
        this.currentStep = 'time';
    }

    setTime(time: string) {
        this.timeScore = generateScore('time', time);
        this.currentStep = 'loading';
        this.errorMessage = '';
        this.currentAlbumIndex = 0;

        const minLoadingTime = 3000;
        const startTime = Date.now();

        const call$ = this.sessionId
            ? this.recommendationService.restart(this.sessionId)
            : this.recommendationService.createSession(this.energyScore, this.familiarityScore, this.timeScore);

        call$.subscribe({
            next: (res) => this.onSessionReceived(res, Date.now() - startTime, minLoadingTime),
            error: (err: HttpErrorResponse) => {
                if (err.status === 429) {
                    this.currentStep = 'limited';
                } else {
                    this.errorMessage = this.buildErrorMessage(err);
                    this.currentStep = 'energy';
                }
            }
        });
    }

    // ── Album actions ─────────────────────────────────────────────────────────

    nextAlbum() {
        if (!this.albums.length || this.isLoadingNext) return;

        this.isLoadingNext = true;
        const nextIndex = (this.currentAlbumIndex + 1) % this.albums.length;
        const nextAlbum = this.albums[nextIndex];

        // Sonraki albümün kapağını preload et
        const img = new Image();
        img.onload = () => {
            this.currentAlbumIndex = nextIndex;
            this.isLoadingNext = false;
        };
        img.onerror = () => {
            // Hata olsa da geçişi bloklama
            this.currentAlbumIndex = nextIndex;
            this.isLoadingNext = false;
        };
        img.src = nextAlbum.coverUrl;
    }



    startOver() {
        if (this.restartsRemaining > 0) {
            this.restartsRemaining--;
            this.energyScore = 0;
            this.familiarityScore = 0;
            this.timeScore = 0;
            this.albums = [];
            this.currentStep = 'energy';
            this.currentAlbumIndex = 0;
        }
    }

    // ── Private ───────────────────────────────────────────────────────────────

    private onSessionReceived(res: SessionResponse, elapsed: number, minTime: number) {
        if (!res.albums?.length) {
            this.errorMessage = 'Uygun album bulunamadi. Lutfen tekrar dene.';
            this.currentStep = 'energy';
            return;
        }

        const remaining = Math.max(0, minTime - elapsed);

        // 1. Resimleri preload et (maksimum bekleme ile)
        const imagePromises = res.albums.map((album) => this.preloadImage(album.coverUrl));

        // 2. Hem minimum süreyi hem de resimlerin yüklenmesini bekle
        Promise.all([
            Promise.all(imagePromises),
            new Promise(resolve => setTimeout(resolve, remaining))
        ]).then(() => {
            this.sessionId = res.sessionId;
            this.albums = res.albums;
            this.captureSeenAlbums(res.albums);
            this.restartsRemaining = res.restartsRemaining;
            this.currentStep = 'album';

            if (this.restartsRemaining === 0) {
                this.saveDailyLimitState(this.seenAlbums.slice(0, 6));
            }
        });
    }

    private preloadImage(url: string, timeoutMs = 5000): Promise<void> {
        return new Promise((resolve) => {
            const img = new Image();
            const timer = setTimeout(() => resolve(), timeoutMs);
            const done = () => {
                clearTimeout(timer);
                resolve();
            };

            img.onload = done;
            img.onerror = done;
            img.src = url;
        });
    }

    private buildErrorMessage(err: HttpErrorResponse): string {
        const errorType: ErrorState =
            err.status === 0 ? 'network' :
                err.status >= 500 ? 'server' : 'unknown';

        if (errorType === 'network') {
            return 'Sunucuya ulasilamadi. Baglantini kontrol edip tekrar dene.';
        }

        if (errorType === 'server') {
            return 'Serviste gecici bir sorun var. Biraz sonra tekrar dene.';
        }

        return 'Istek tamamlanamadi. Lutfen tekrar dene.';
    }

    private captureSeenAlbums(albums: Album[]): void {
        for (const album of albums) {
            if (!this.seenAlbums.some((x) => x.id === album.id)) {
                this.seenAlbums.push(album);
            }
        }
    }

    private getDailyLimitState(): { locked: boolean; albums: Album[] } | null {
        try {
            const raw = localStorage.getItem(this.dailyLimitStorageKey());
            if (!raw) return null;
            return JSON.parse(raw) as { locked: boolean; albums: Album[] };
        } catch {
            return null;
        }
    }

    private saveDailyLimitState(albums: Album[]): void {
        const payload = JSON.stringify({ locked: true, albums });
        localStorage.setItem(this.dailyLimitStorageKey(), payload);
    }

    private dailyLimitStorageKey(): string {
        const anonId = this.recommendationService.getOrCreateAnonId();
        const now = new Date();
        const yyyy = now.getFullYear();
        const mm = String(now.getMonth() + 1).padStart(2, '0');
        const dd = String(now.getDate()).padStart(2, '0');
        return `music_daily_limit:${anonId}:${yyyy}-${mm}-${dd}`;
    }

    private clearAllDailyLimitKeys(): void {
        const prefix = 'music_daily_limit:';
        const keysToRemove = Object.keys(localStorage).filter(k => k.startsWith(prefix));
        keysToRemove.forEach(k => localStorage.removeItem(k));
    }

    trackByAlbumId(_: number, album: Album): string {
        return album.id;
    }
}
