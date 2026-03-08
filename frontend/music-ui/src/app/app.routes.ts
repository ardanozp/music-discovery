import { Routes } from '@angular/router';
import { AlbumScreenComponent } from './album-screen/album-screen.component';

export const routes: Routes = [
  { path: '', component: AlbumScreenComponent },
  { path: '**', redirectTo: '' },
];
