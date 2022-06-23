import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.scss']
})

export class EventosComponent implements OnInit {

  public eventos: any = [];
  public eventsFilters:any = [];

  isImg: boolean = true;
  imgSize: number = 160;
  private _filterList: string = '';

  public get filterList() {
    return this._filterList
  }

  public set filterList(value: string) {
    this._filterList = value;
    this.eventsFilters = this.filterList ? this.filterEvents(this.filterList) : this.eventos;
  }

  filterEvents(filterBy: string): any {
    filterBy = filterBy.toLocaleLowerCase();
    return this.eventos.filter(
      (evento: { tema: string; local: string }) => evento.tema.toLocaleLowerCase().indexOf(filterBy) != -1 ||
        evento.local.toLocaleLowerCase().indexOf(filterBy) != -1
    )
  }

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.getEventos()
  }

  public getEventos(): void {
    this.http.get('https://localhost:5001/api/evento').subscribe(
      response => {
        this.eventos = response,
        this.eventsFilters = response
      },
      error => console.log(error)
    )
  }

  public updateImageStage(): void {
    this.isImg = !this.isImg
  }
}
