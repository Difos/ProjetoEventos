import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ConfirmarSenha } from '@app/helpers/CofirmarSenha';
import { User } from '@app/models/identity/User';
import { AccountService } from '@app/services/Account.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.scss']
})
export class RegistrationComponent implements OnInit {

  user = {} as User
  form!:FormGroup

  public get f():any{
    return this.form.controls
  }

  constructor(
    private formBuilder: FormBuilder,
    private accountService: AccountService,
    private router: Router,
    private toastr: ToastrService) { }

  ngOnInit(): void {
    this.validation();
  }

  public validation(): void {


    this.form = this.formBuilder.group({
      primeiroNome:['',[Validators.required,Validators.minLength(4),Validators.maxLength(20)]],
      segundoNome:['',[Validators.required,Validators.minLength(4),Validators.maxLength(20)]],
      email:['',[Validators.required,Validators.email]],
      userName:['',Validators.required],
      password:['',[Validators.required]],
      confirmarPassword:['',[Validators.required, Validators.minLength(4)]]
    },{
      validator: ConfirmarSenha('password','confirmarPassword')
    });
  }

  register(): void {
    this.user = { ...this.form.value };
    console.log(this.user)
    this.accountService.register(this.user).subscribe(
      () => {
        this.router.navigateByUrl('/dashboard');
      },
      (error: any) => {
        this.toastr.error(error.message);
      }
    )
  }

}
