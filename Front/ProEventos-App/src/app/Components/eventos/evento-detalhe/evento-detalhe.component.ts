import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';

import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/Evento.service';

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



  constructor(private formBuilder: FormBuilder,
    private localeService: BsLocaleService,
    private router: ActivatedRoute,
    private eventoService: EventoService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService) {
    this.localeService.use('pt-br')
  }

  public carregarEvento(): void {
    const eventoIdParam = this.router.snapshot.paramMap.get('id');



    if (eventoIdParam != null) {
      this.saveState = 'put';
      this.spinner.show();
      this.eventoService.getEventoId(+eventoIdParam).subscribe(
        (evento: Evento) => {
          this.evento = { ...evento }
          this.form.patchValue(this.evento);
        },
        (error: any) => {
          this.spinner.hide();
          this.toastr.error('Error ao tentar carregar evento', 'Erro!')
          console.error(error);
        },
        () => this.spinner.hide()
      )
    }
  }
  ngOnInit(): void {
    this.validation();
    this.carregarEvento();

  }

  public validation(): void {
    this.form = this.formBuilder.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      imagemURL: ['', Validators.required],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl): any {
    return { 'is-invalid': campoForm?.errors && campoForm?.touched }
  }

  public salvarAlteracao(): void {

    this.spinner.show();

    if (this.form.valid) {

      this.evento = (this.saveState === 'post')
        ? { ...this.form.value }
        : { id: this.evento.id, ...this.form.value }

      this.eventoService[this.saveState](this.evento).subscribe(
        () => this.toastr.success('Evento salvo com sucesso', 'success'),
        (error: any) => {
          console.error(error);
          this.spinner.hide()
          this.toastr.error('Erro ao salvar o evento', 'error')
        },
        () => this.spinner.hide()
      );
    }
  }
}

