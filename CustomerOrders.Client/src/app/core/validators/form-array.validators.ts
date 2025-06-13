import { AbstractControl, FormArray, ValidatorFn } from '@angular/forms';

export function formArrayMinLengthValidator(min: number): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    if (!(control instanceof FormArray)) {
      return null;
    }

    const array = control as FormArray;
    const length = array.length;

    if (length < min) {
      return { minLength: { requiredLength: min, actualLength: length } };
    }

    return null;
  };
}
