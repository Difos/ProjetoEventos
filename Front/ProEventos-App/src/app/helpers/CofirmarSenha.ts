import {FormGroup} from '@angular/forms';

export function ConfirmarSenha(nomeControle:string, confirmacaoNomeControle:string){
  return (formGroup:FormGroup) => {
    const control = formGroup.controls[nomeControle];
    const confirmacaoControle = formGroup.controls[confirmacaoNomeControle];


    if(confirmacaoControle.errors && !confirmacaoControle.errors.confirmarSenha){

      return;
    }

    if(control.value !== confirmacaoControle.value){
      confirmacaoControle.setErrors({confirmarSenha : true});

    }else{
      confirmacaoControle.setErrors(null)
    }
  }
}
