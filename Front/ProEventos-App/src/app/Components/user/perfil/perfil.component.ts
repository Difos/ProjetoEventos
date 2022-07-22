import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmarSenha } from '@app/helpers/CofirmarSenha';

@Component({
  selector: 'app-perfil',
  templateUrl: './perfil.component.html',
  styleUrls: ['./perfil.component.scss']
})
export class PerfilComponent implements OnInit {

  form!: FormGroup

  public get f(){
    return this.form.controls
  }

  constructor(private formBuilder: FormBuilder) { }


  ngOnInit(): void {
    this.validation();
  }

  public validation():void{

    this.form = this.formBuilder.group({
      titulo:['',[Validators.required]],
      primeiroNome:['',[Validators.required,Validators.minLength(4),Validators.maxLength(20)]],
      ultimoNome:['',[Validators.required,Validators.minLength(4),Validators.maxLength(20)]],
      email:['',[Validators.required,Validators.email]],
      telefone:['',Validators.required],
      funcao:['',Validators.required],
      descricao:['',[Validators.required]],
      senha:['',[Validators.required]],
      confirmarSenha:['',[Validators.required]]
    },{
      validator: ConfirmarSenha('senha','confirmarSenha')
    });
  }

}
