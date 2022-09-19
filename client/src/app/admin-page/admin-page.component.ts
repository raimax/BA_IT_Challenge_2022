import { Component, OnInit } from '@angular/core';
import { MenuItem } from 'primeng/api';

@Component({
  selector: 'app-admin-page',
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.scss'],
})
export class AdminPageComponent implements OnInit {
  items: MenuItem[] = [];
  activeItem: MenuItem = { label: 'Reserved Books', icon: 'pi pi-fw pi-clock' };

  constructor() {}

  ngOnInit(): void {
    this.items = [
      {
        label: 'Reserved Books',
        icon: 'pi pi-fw pi-clock',
        command: () => (this.activeItem = this.items[0]),
      },
      {
        label: 'Borrowed Books',
        icon: 'pi pi-fw pi-book',
        command: () => (this.activeItem = this.items[1]),
      },
    ];
    this.activeItem = this.items[0];
  }
}
