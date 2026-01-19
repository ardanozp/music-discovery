import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecommendationService } from '../services/recommendation.service';
import { Album } from '../models/album.model';

@Component({
    selector: 'app-album-screen',
    imports: [CommonModule],
    templateUrl: './album-screen.component.html',
    styleUrl: './album-screen.component.css'
})
export class AlbumScreenComponent {
    // State: Albüm listesi (Servisten gelecek)
    albums: Album[] = [];

    // State: Hangi moddayız? '' (Boş) ise Soru Ekranı, doluysa Albüm Ekranı
    selectedMood = '';

    // State: Sıra numarası
    currentAlbumIndex = 0;

    constructor(private recommendationService: RecommendationService) { }

    // Helper: Ekrandaki albümü seç
    get currentAlbum() {
        if (this.albums.length === 0) return null;
        return this.albums[this.currentAlbumIndex];
    }

    // Action: Modu değiştir (ve Servisten albümleri çek)
    setMood(mood: string) {
        this.selectedMood = mood;
        this.currentAlbumIndex = 0; // Başa dön

        // Servis çağrısı
        this.recommendationService.getRecommendedAlbums(mood).subscribe(response => {
            this.albums = response.albums;
        });
    }

    // Action: Sonraki albüm
    nextAlbum() {
        if (this.albums.length === 0) return;
        this.currentAlbumIndex = (this.currentAlbumIndex + 1) % this.albums.length;
    }

    // Action: Restart (Başa sar)
    reset() {
        this.selectedMood = ''; // Modu sıfırla (Soru ekranını açar)
        this.currentAlbumIndex = 0;
        this.albums = []; // Listeyi temizle
    }
}
