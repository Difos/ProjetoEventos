import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {Observable} from 'rxjs';
import { Lote } from '../models/Lotes';
import {take} from 'rxjs/operators';

@Injectable()
export class LoteService {

  baseURL = 'https://localhost:5001/api/lotes'

  constructor(private http: HttpClient) { }

  public getLotesByEventoId(eventoId: number):Observable<Lote[]> {
    return this.http.get<Lote[]>(`${this.baseURL}/${eventoId}`)
    .pipe(take(1))
  }

  public saveLote(eventoId: number, lotes: Lote[]): Observable<Lote[]> {
    // alert(JSON.stringify(evento)+'id:"'+evento.id)
    return this.http.put<Lote[]>(`${this.baseURL}/${eventoId}`,lotes)
    .pipe(take(1))
  }

  public deleteLote(loteId: number, eventoid: number):Observable<any> {
    return this.http.delete(`${this.baseURL}/${eventoid}/${loteId}`)
    .pipe(take(1))
  }

}
