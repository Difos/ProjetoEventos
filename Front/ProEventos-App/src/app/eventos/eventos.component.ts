import { Component, OnInit } from '@angular/core';
import { Evento } from '../models/Evento';
import { EventoService } from '../services/Evento.service';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})

export class EventosComponent implements OnInit {

  public eventos: Evento[] = [];
  public eventsFilters: Evento[] = [];

  public isImg: boolean = true;
  public imgSize: number = 160;
  private _filterList: string = '';

  public get filterList() {
    return this._filterList
  }

  public set filterList(value: string) {
    this._filterList = value;
    this.eventsFilters = this.filterList ? this.filterEvents(this.filterList) : this.eventos;
  }

  public filterEvents(filterBy: string): Evento[] {
    filterBy = filterBy.toLocaleLowerCase();
    return this.eventos.filter(
      evento => evento.tema.toLocaleLowerCase().indexOf(filterBy) != -1 ||
        evento.local.toLocaleLowerCase().indexOf(filterBy) != -1
    )
  }

  constructor(private eventoService: EventoService) { }

  public ngOnInit(): void {
    this.getEventos()
  }

  public getEventos(): void {
    this.eventoService.getEventos().subscribe({
      next: (eventos: Evento[]) => {
        this.eventos = eventos,
          this.eventsFilters = eventos
      },
      error: (error: any) => console.log(error)
    });
  }

  public updateImageStage(): void {
    this.isImg = !this.isImg
  }
}
