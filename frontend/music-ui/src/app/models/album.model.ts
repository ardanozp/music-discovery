export interface Album {
    title: string;
    artist: string;
    year: number;
    coverUrl: string;
    wikipediaUrl: string;
    tag: string;
    energyScore?: number;
    emotionScore?: number;
    familiarityScore?: number;
    timeScore?: number;
}
