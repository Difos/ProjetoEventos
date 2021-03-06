import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';

import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from 'src/app/models/Evento';
import { EventoService } from 'src/app/services/Evento.service';




@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  modalRef?: BsModalRef;
  message: string = "";

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


  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router
  ) { }

  public ngOnInit(): void {
    this.spinner.show()
    this.getEventos()

  }

  public getEventos(): void {
    this.eventoService.getEventos().subscribe({
      next: (eventos: Evento[]) => {
        this.eventos = eventos,
          this.eventsFilters = eventos
      },
      error: (error: any) => {
        this.spinner.hide();
        this.toastr.error('Erro ao carregar os eventos!', 'Error')
      },
      complete: () => this.spinner.hide()

    });
  }

  public updateImageStage(): void {
    this.isImg = !this.isImg
  }

  openModal(template: TemplateRef<any>): void {
    this.modalRef = this.modalService.show(template, { class: 'modal-sm' });
  }

  confirm(): void {
    this.toastr.success('O Evento foi deletado com sucesso', 'Deletado!')
    this.modalRef?.hide();
  }

  decline(): void {
    this.toastr.warning('Opera????o cancelada!', 'Cancelado')
    this.modalRef?.hide();
  }

  detalheEvento(id:number):void{
    this.router.navigate([`eventos/detalhe/${id}`])
  }
}
