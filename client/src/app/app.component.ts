import { Component } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';
import { PrimeNGConfig } from 'primeng/api';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  constructor(
    private accountService: AccountService,
    private primengConfig: PrimeNGConfig
  ) {}

  ngOnInit(): void {
    this.setCurrentUser();
    this.primengConfig.ripple = true;
  }

  setCurrentUser() {
    const storageItem: string | null = localStorage.getItem('user');

    if (storageItem != null) {
      const user: User = JSON.parse(storageItem);

      if (user != null) {
        this.accountService.setCurrentUser(user);
      }
    }
  }
}
