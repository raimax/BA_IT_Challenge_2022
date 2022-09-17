import { Component } from '@angular/core';
import { User } from './_models/user';
import { AccountService } from './_services/account.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
})
export class AppComponent {
  constructor(private accountService: AccountService) {}

  ngOnInit(): void {
    this.setCurrentUser();
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
