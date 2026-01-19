import { Component } from '@angular/core';
import { AlbumScreenComponent } from './album-screen/album-screen.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [AlbumScreenComponent],
  template: `<app-album-screen />`
})
export class AppComponent { }
