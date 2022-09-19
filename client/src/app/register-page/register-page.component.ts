import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { CustomValidators } from '../validators/CustomValidators';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register-page',
  templateUrl: './register-page.component.html',
  styleUrls: ['./register-page.component.scss'],
})
export class RegisterPageComponent implements OnInit {
  isLoading: boolean = false;

  constructor(
    private accountService: AccountService,
    private router: Router,
    private fb: FormBuilder
  ) {}

  ngOnInit(): void {}

  registerForm = this.fb.group(
    {
      username: ['', [Validators.required]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      repeatPassword: ['', [Validators.required]],
    },
    {
      validators: CustomValidators.match('password', 'repeatPassword'),
    }
  );

  register() {
    this.isLoading = true;
    this.accountService.register(this.registerForm.value).subscribe({
      next: () => {
        this.isLoading = false;
        this.router.navigateByUrl('/search');
      },
      error: (error) => {
        console.log(error);
        this.isLoading = false;
      },
    });
  }
}
