import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { trigger, transition, style, animate } from '@angular/animations';
import { RecommendationService } from '../services/recommendation.service';
import { Album } from '../models/album.model';

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
        ])
    ]
})
export class AlbumScreenComponent {
    // State: Albüm listesi (Servisten gelecek)
    albums: Album[] = [];

    // State: Seçilen energy
    selectedEnergy = '';

    // State: Seçilen emotion
    selectedEmotion = '';

    // State: Seçilen familiarity
    selectedFamiliarity = '';

    // State: Seçilen time
    selectedTime = '';

    // State: Hangi adımdayız?
    currentStep: 'energy' | 'emotion' | 'familiarity' | 'time' | 'loading' | 'album' = 'energy';

    // State: Sıra numarası
    currentAlbumIndex = 0;

    constructor(private recommendationService: RecommendationService) { }

    // Helper: Ekrandaki albümü seç
    get currentAlbum() {
        if (this.albums.length === 0) return null;
        return this.albums[this.currentAlbumIndex];
    }

    // Action: Energy seç ve emotion sorusuna geç
    setEnergy(energy: string) {
        this.selectedEnergy = energy;
        this.currentStep = 'emotion';
    }

    // Action: Emotion seç ve familiarity sorusuna geç
    setEmotion(emotion: string) {
        this.selectedEmotion = emotion;
        this.currentStep = 'familiarity';
    }

    // Action: Familiarity seç ve time sorusuna geç
    setFamiliarity(familiarity: string) {
        this.selectedFamiliarity = familiarity;
        this.currentStep = 'time';
    }

    // Action: Time seç ve backend'den albümleri çek
    setTime(time: string) {
        this.selectedTime = time;
        this.currentAlbumIndex = 0;

        // Loading state'e geç
        this.currentStep = 'loading';

        // Minimum loading süresi için delay
        const minLoadingTime = 3000; // 3 saniye
        const startTime = Date.now();

        // Servis çağrısı (tüm parametreler gönderiliyor)
        this.recommendationService.getRecommendedAlbums(
            this.selectedEnergy,
            this.selectedEmotion,
            this.selectedFamiliarity,
            time
        ).subscribe(response => {
            const elapsedTime = Date.now() - startTime;
            const remainingTime = Math.max(0, minLoadingTime - elapsedTime);

            // Minimum süre dolmadan albüm ekranına geçme
            setTimeout(() => {
                this.albums = response.albums;
                this.currentStep = 'album';
            }, remainingTime);
        });
    }

    // Action: Sonraki albüm
    nextAlbum() {
        if (this.albums.length === 0) return;
        this.currentAlbumIndex = (this.currentAlbumIndex + 1) % this.albums.length;
    }

    // Action: Restart (Başa sar)
    reset() {
        this.selectedEnergy = '';
        this.selectedEmotion = '';
        this.selectedFamiliarity = '';
        this.selectedTime = '';
        this.currentStep = 'energy';
        this.currentAlbumIndex = 0;
        this.albums = [];
    }
}
