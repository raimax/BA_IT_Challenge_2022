import { AbstractControl, ValidatorFn } from '@angular/forms';

export class CustomValidators {
  constructor() {}

  static match(controlName: string, checkControlName: string): ValidatorFn {
    return (controls: AbstractControl) => {
      const control = controls.get(controlName);
      const checkControl = controls.get(checkControlName);

      if (checkControl === null || control === null) return null;

      if (checkControl.errors && !checkControl.errors['matching']) {
        return null;
      }

      if (control.value !== checkControl.value) {
        checkControl.setErrors({ matching: true });
        return { matching: true };
      } else {
        return null;
      }
    };
  }
}
