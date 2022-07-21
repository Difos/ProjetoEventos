import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ConfirmarSenha } from '@app/helpers/CofirmarSenha';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {


  form!:FormGroup

  get f():any{
    return this.form.controls
  }

  constructor(private formBuilder: FormBuilder) { }

  ngOnInit(): void {
    this.validation();
  }

  public validation(): void {
    this.form = this.formBuilder.group({
      primeiroNome:['',[Validators.required,Validators.minLength(4),Validators.maxLength(20)]],
      segundoNome:['',[Validators.required,Validators.minLength(4),Validators.maxLength(20)]],
      email:['',[Validators.required,Validators.email]],
      usuario:['',Validators.required],
      senha:['',[Validators.required]],
      confirmarSenha:['',[Validators.required]]
    },{
      validator: ConfirmarSenha('senha','confirmarSenha')
    });
  }

}
