import { inject, Service } from '@angular/core';
import  {HttpClient} from '@angular/common/http';
import {League} from '../models/leagues';
import {Team} from '../models/teams';

@Service()
export class LeagueService {
  private http = inject(HttpClient);
  private url = 'http://localhost:5137/api/leagues';

  getAll(){
    return this.http.get<League[]>(this.url);
  }

  getOne(id :number){
    return this.http.get<League>(`${this.url}/${id}`);
  }

  create(league: League){
    return this.http.post<League>(this.url, league);
  }

  update(league: League){
    return this.http.put<void>(`${this.url}/${league.id}`, league);
  }

  delete(id: number){
    return this.http.delete<void>(`${this.url}/${id}`);
  }

  getTeams(leagueId: number){
    return this.http.get<Team[]>(`${this.url}/${leagueId}/teams`);
  }

  addTeam(leagueId: number, teamId: number){
    return this.http.post<void>(`${this.url}/${leagueId}/teams/${teamId}`, null);
  }

  removeTeam(leagueId: number, teamId: number){
    return this.http.delete<void>(`${this.url}/${leagueId}/teams/${teamId}`);
  }
}
