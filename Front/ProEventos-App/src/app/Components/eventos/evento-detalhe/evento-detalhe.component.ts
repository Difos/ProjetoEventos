import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lotes';
import { EventoService } from '@app/services/Evento.service';
import { LoteService } from '@app/services/Lote.service';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {
  modalRef: BsModalRef;
  form!: FormGroup;
  evento = {} as Evento;
  saveState = 'post';
  eventoId: number;
  loteAtual = {id: 0, nome: '', indice: 0}

  get f(): any {
    return this.form.controls
  }

  get bsConfig(): any {
    return {
      isAnimated: true,
      dateInputFormat: 'DD/MM/YYYY HH:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false
    }
  }

  get bsConfigLote(): any {
    return {
      isAnimated: true,
      dateInputFormat: 'DD/MM/YYYY',
      containerClass: 'theme-default',
      showWeekNumbers: false
    }
  }

  get modoEditar(): boolean {
    return this.saveState === 'put'
  }

  get lotes(): FormArray {
    return this.form.get('lotes') as FormArray
  }

  constructor(private formBuilder: FormBuilder,
    private localeService: BsLocaleService,
    private router: ActivatedRoute,
    private eventoService: EventoService,
    private loteService: LoteService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private modalService: BsModalService,
    private routerA: Router) {
    this.localeService.use('pt-br')
  }

  public carregarEvento(): void {
    this.eventoId = +this.router.snapshot.paramMap.get('id');

    if (this.eventoId != null && this.eventoId != 0) {
      this.saveState = 'put';
      this.spinner.show();
      this.eventoService.getEventoId(this.eventoId).subscribe(
        (evento: Evento) => {
          this.evento = { ...evento }
          this.form.patchValue(this.evento);
          // this.carregarLotes()
          this.evento.lotes.forEach(lote =>{
            this.lotes.push(this.criarLote(lote))
          })
        },
        (error: any) => {
          this.toastr.error('Error ao tentar carregar evento', 'Erro!')
          console.error(error);
        },
      ).add(() => this.spinner.hide())
    }
  }

  ngOnInit(): void {
    this.validation();
    this.carregarEvento();

  }

  public confirm(): void{
    this.modalRef.hide();
    this.spinner.show();
    this.loteService.deleteLote(this.eventoId, this.loteAtual.id)
    .subscribe(
      () => {
        this.toastr.success('batch has been deleted')
      },
      (error) => {
        this.toastr.error(`erro to try delete the batch ${this.loteAtual.nome}`)
      }
    ).add(() => this.spinner.hide())
  }
  public decline(): void {
    this.modalRef.hide();
  }

  public removerLote(template: TemplateRef<any>, indice: number): void{

    this.loteAtual.id = this.lotes.get(indice + '.id').value
    this.loteAtual.nome = this.lotes.get(indice + '.nome').value
    this.loteAtual.indice = indice

    this.modalRef = this.modalService.show(template, {class: 'modal-sm'})

    this.lotes.removeAt(indice);

  }

  public carregarLotes(): void {
    this.loteService.getLotesByEventoId(this.eventoId).subscribe(
      (lotesRetorno: Lote[]) => {
        lotesRetorno.forEach(lote => {
          this.lotes.push(this.criarLote(lote))
          this.lotes.removeAt(this.loteAtual.indice)
        })
      },
      (error: any) => {
        this.toastr.error('error to try loading batchs')
        console.log(error)
      }
    ).add(() => this.spinner.hide())
  }

  public validation(): void {
    this.form = this.formBuilder.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      imagemURL: ['', Validators.required],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      lotes: this.formBuilder.array([])
    });
  }

  adicionarLote(): void {
    this.lotes.push(
      this.criarLote({id: 0} as Lote)
    )
  }

  criarLote(lote: Lote): FormGroup {
    return this.formBuilder.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio:[lote.dataInicio],
      dataFim:[lote.dataFim],
      qtd: [lote.qtd, Validators.required],
    })
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl | AbstractControl): any {
    return { 'is-invalid': campoForm?.errors && campoForm?.touched }
  }

  public mudarValorData(value: Date, indice: number, campo: string): void {
    this.lotes.value[indice][campo] = value;
  }

  public salvarEvento(): void {

    this.spinner.show();

    if (this.form.valid) {

      this.evento = (this.saveState === 'post')
        ? { ...this.form.value }
        : { id: this.evento.id, ...this.form.value }

      this.eventoService[this.saveState](this.evento).subscribe(
        (eventoRetorno: Evento) => {
          this.toastr.success('Evento salvo com sucesso', 'success')
          this.routerA.navigate([`eventos/detalhe/${eventoRetorno.id}`])
        },
        (error: any) => {
          console.error(error);
          this.spinner.hide()
          this.toastr.error('Erro ao salvar o evento', 'error')
        },
        () => {
          this.spinner.hide()
        }

      );
    }
  }

  public returnTituloLote(titulo: string): string {
    return titulo === null || titulo =='' ? titulo : 'Nome do lote'
  }

  public salvarLotes(): void {
    if(this.form.controls.lotes.valid){
      this.spinner.show();
      this.loteService.saveLote(this.eventoId, this.form.value.lotes)
      .subscribe(
        () => {
          this.toastr.success('batch has be saved successful','success')
          this.lotes.reset();
        },
        (error: any) => {
          this.toastr.error('error to try save batchs')
          console.error(error);
        },
      ).add(() => {
        this.spinner.hide()
      })
    }
  }
}

