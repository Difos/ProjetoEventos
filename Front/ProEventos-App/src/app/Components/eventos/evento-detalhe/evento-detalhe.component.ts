import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';

import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lotes';
import { EventoService } from '@app/services/Evento.service';
import { LoteService } from '@app/services/Lote.service';

import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';


@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})
export class EventoDetalheComponent implements OnInit {

  form!: FormGroup;
  evento = {} as Evento;
  saveState = 'post';
  eventoId: number;

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
    private routerA: Router) {
    this.localeService.use('pt-br')
  }

  public carregarEvento(): void {
    this.eventoId = +this.router.snapshot.paramMap.get('id');



    if (this.eventoId != null || this.eventoId == 0) {
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

  public carregarLotes(): void {
    this.loteService.getLotesByEventoId(this.eventoId).subscribe(
      (lotesRetorno: Lote[]) => {
        lotesRetorno.forEach(lote => {
          this.lotes.push(this.criarLote(lote))
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

  public salvarLotes(): void {
    this.spinner.show();
    if(this.form.controls.lotes.valid){
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

