import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ConfirmarSenha } from '@app/helpers/CofirmarSenha';
import { UserUpdate } from '@app/models/identity/UserUpdate';
import { AccountService } from '@app/services/Account.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {
  userUpdate = {} as UserUpdate;
  form!: FormGroup

  public get f(){
    return this.form.controls
  }

  constructor(private formBuilder: FormBuilder,
              public accountService: AccountService,
              private router: Router,
              private toast: ToastrService,
              private spinner: NgxSpinnerService) { }

  ngOnInit(): void {
    this.validation();
    this.carregarUsuario();
  }

  public validation(): void {

    this.form = this.formBuilder.group({
      userName: [''],
      titulo:['Não Informado',[Validators.required]],
      primeiroNome:['',[Validators.required,Validators.minLength(4),Validators.maxLength(20)]],
      ultimoNome:['',[Validators.required,Validators.minLength(4),Validators.maxLength(20)]],
      email:['',[Validators.required,Validators.email]],
      phoneNumber:['',Validators.required],
      funcao:['NaoInformado',Validators.required],
      descricao:['',[Validators.required]],
      password:['',[Validators.required]],
      confirmarPassword:['',[Validators.required]]
    },{
      validator: ConfirmarSenha('password','confirmarPassword')
    });
  }

  private carregarUsuario(): void {
    this.spinner.show();
    this.accountService.getUser().subscribe(
      (userReturn: UserUpdate) => {
        console.log(userReturn)
        this.userUpdate = userReturn
        this.form.patchValue(this.userUpdate)
        this.toast.success('user has been loaded!','success')
      },
      (error) => {
        console.error(error)
        this.toast.error('user not loaded!','error')
        this.router.navigate(['/dashboard'])
      }
    )
    .add(() => this.spinner.hide());
  }

  onSubmit(): void {
    this.atualizarUsuario()
  }

  public atualizarUsuario(): void {
    this.userUpdate = { ...this.form.value }
    this.spinner.show();
    this.accountService.updateUser(this.userUpdate).subscribe(
      () => {
        this.toast.success('user has been updated!','success')
      },
      (error) => {
        this.toast.error('user not updated!','error')
      }
    ).add(() => this.spinner.hide());
  }
}
