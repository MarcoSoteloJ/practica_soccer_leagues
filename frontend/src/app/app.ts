import { Component, signal } from '@angular/core';
import {Teams} from './pages/teams/teams';
import {Leagues} from './pages/leagues/leagues';

@Component({
  imports: [Teams, Leagues],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  activeTab = signal('teams');
}
