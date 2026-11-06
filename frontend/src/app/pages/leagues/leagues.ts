import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { League } from '../../models/leagues';
import { Team } from '../../models/teams';
import { LeagueService } from '../../services/league-service';
import { TeamService } from '../../services/team-service';

@Component({
  standalone: true,
  imports: [FormsModule],
  selector: 'app-leagues',
  styleUrl: './leagues.css',
  templateUrl: './leagues.html',
})
export class Leagues implements OnInit {
  private leagueService = inject(LeagueService);
  private teamService = inject(TeamService);

  leagues = signal<League[]>([]);
  editingId = signal<number | null>(null);
  errorMessage = signal('');

  selectedLeague = signal<League | null>(null);
  leagueTeams = signal<Team[]>([]);
  allTeams = signal<Team[]>([]);
  teamToAdd = signal<number | null>(null);

  availableTeams = computed(() => {
    const leagueTeamIds = this.leagueTeams().map((team) => team.id);
    return this.allTeams().filter((team) => !leagueTeamIds.includes(team.id));
  });

  name = signal('');
  country = signal('');
  startDate = signal('');
  endDate = signal('');
  enabled = signal(true);

  ngOnInit() {
    this.load();
  }

  load() {
    this.leagueService.getAll().subscribe((data) => {
      this.leagues.set(data);
    });
  }

  save(form: NgForm) {
    this.errorMessage.set('');

    const league: League = {
      id: this.editingId() ?? 0,
      name: this.name(),
      country: this.country(),
      startDate: this.startDate(),
      endDate: this.endDate(),
      enabled: this.enabled(),
    };

    if (this.editingId() === null) {
      this.leagueService.create(league).subscribe({
        next: () => this.afterSave(form),
        error: (err) => this.showError(err),
      });
    } else {
      this.leagueService.update(league).subscribe({
        next: () => this.afterSave(form),
        error: (err) => this.showError(err),
      });
    }
  }

  edit(league: League) {
    this.errorMessage.set('');
    this.editingId.set(league.id);
    this.name.set(league.name);
    this.country.set(league.country);
    this.startDate.set(league.startDate.slice(0, 10));
    this.endDate.set(league.endDate.slice(0, 10));
    this.enabled.set(league.enabled);
  }

  remove(id: number) {
    if (confirm('Delete this league?')) {
      this.leagueService.delete(id).subscribe(() => {
        this.closeTeams();
        this.load();
      });
    }
  }

  manageTeams(league: League) {
    this.selectedLeague.set(league);
    this.teamToAdd.set(null);
    this.loadLeagueTeams(league.id);
    this.teamService.getAll().subscribe((data) => {
      this.allTeams.set(data);
    });
  }

  closeTeams() {
    this.selectedLeague.set(null);
  }

  addTeam(leagueId: number) {
    const teamId = this.teamToAdd();

    if (teamId !== null) {
      this.leagueService.addTeam(leagueId, teamId).subscribe(() => {
        this.teamToAdd.set(null);
        this.loadLeagueTeams(leagueId);
      });
    }
  }

  removeTeam(leagueId: number, teamId: number) {
    this.leagueService.removeTeam(leagueId, teamId).subscribe(() => {
      this.loadLeagueTeams(leagueId);
    });
  }

  clearForm(form: NgForm) {
    this.errorMessage.set('');
    this.editingId.set(null);
    form.resetForm({ name: '', country: '', startDate: '', endDate: '', enabled: true });
  }

  private afterSave(form: NgForm) {
    this.clearForm(form);
    this.load();
  }

  private loadLeagueTeams(leagueId: number) {
    this.leagueService.getTeams(leagueId).subscribe((data) => {
      this.leagueTeams.set(data);
    });
  }

  private showError(err: { status: number; error: string }) {
    this.errorMessage.set(err.status === 400 ? err.error : 'Could not save the league');
  }
}
