import { inject, Service } from '@angular/core';
import  {HttpClient} from '@angular/common/http';
import {Team} from '../models/teams';
import {League} from '../models/leagues';

@Service()
export class TeamService {
  private http = inject(HttpClient);
  private url = 'http://localhost:5137/api/teams';

  getAll(){
    return this.http.get<Team[]>(this.url);
  }

  getOne(id :number){
    return this.http.get<Team>(`${this.url}/${id}`);
  }

  create(team: Team){
    return this.http.post<Team>(this.url, team);
  }

  update(team: Team){
    return this.http.put<void>(`${this.url}/${team.id}`, team);
  }

  delete(id: number){
    return this.http.delete<void>(`${this.url}/${id}`);
  }

  getLeagues(teamId: number){
    return this.http.get<League[]>(`${this.url}/${teamId}/leagues`);
  }
}
