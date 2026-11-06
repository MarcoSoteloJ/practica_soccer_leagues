import { Component, inject, OnInit, signal } from '@angular/core';
import { FormsModule, NgForm } from '@angular/forms';
import { League } from '../../models/leagues';
import { Team } from '../../models/teams';
import { TeamService } from '../../services/team-service';

@Component({
  standalone: true,
  imports: [FormsModule],
  selector: 'app-teams',
  styleUrl: './teams.css',
  templateUrl: './teams.html',
})
export class Teams implements OnInit {
  private teamService = inject(TeamService);

  teams = signal<Team[]>([]);
  editingId = signal<number | null>(null);
  selectedTeam = signal<Team | null>(null);
  teamLeagues = signal<League[]>([]);

  name = signal('');
  country = signal('');
  playersQuantity = signal(11);
  enabled = signal(true);

  ngOnInit() {
    this.load();
  }

  load() {
    this.teamService.getAll().subscribe((data) => {
      this.teams.set(data);
    });
  }

  save(form: NgForm) {
    const team: Team = {
      id: this.editingId() ?? 0,
      name: this.name(),
      country: this.country(),
      playersQuantity: this.playersQuantity(),
      enabled: this.enabled(),
    };

    if (this.editingId() === null) {
      this.teamService.create(team).subscribe(() => this.afterSave(form));
    } else {
      this.teamService.update(team).subscribe(() => this.afterSave(form));
    }
  }

  edit(team: Team) {
    this.editingId.set(team.id);
    this.name.set(team.name);
    this.country.set(team.country);
    this.playersQuantity.set(team.playersQuantity);
    this.enabled.set(team.enabled);
  }

  remove(id: number) {
    if (confirm('Delete this team?')) {
      this.teamService.delete(id).subscribe(() => {
        this.closeLeagues();
        this.load();
      });
    }
  }

  showLeagues(team: Team) {
    this.selectedTeam.set(team);
    this.teamService.getLeagues(team.id).subscribe((data) => {
      this.teamLeagues.set(data);
    });
  }

  closeLeagues() {
    this.selectedTeam.set(null);
  }

  clearForm(form: NgForm) {
    this.editingId.set(null);
    form.resetForm({ name: '', country: '', playersQuantity: 11, enabled: true });
  }

  private afterSave(form: NgForm) {
    this.clearForm(form);
    this.load();
  }
}
