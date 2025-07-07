import { Component, inject, signal } from '@angular/core';
import { MenuItem } from 'primeng/api';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { StyleClassModule } from 'primeng/styleclass';
import { LayoutService } from '../service/layout.service';
import { SelectModule } from 'primeng/select';
import { FormsModule } from '@angular/forms';
import { ButtonModule } from 'primeng/button';
import {TranslateModule} from "@ngx-translate/core";
import { LanguageService } from '../../core/services/language.service';

interface ILanguageOption {
   name: string;
   code:string;
   img: string;
}

@Component({
  selector: 'app-topbar',
  standalone: true,
  imports: [RouterModule, CommonModule, StyleClassModule, SelectModule, FormsModule, ButtonModule, TranslateModule],
  template: ` <div class="layout-topbar">
    <div class="layout-topbar-logo-container">
      <button
        class="layout-menu-button layout-topbar-action"
        (click)="layoutService.onMenuToggle()"
      >
        <i class="pi pi-bars"></i>
      </button>
      <a class="layout-topbar-logo" routerLink="/">
        <img src="tfst_logo_a.png" />
      </a>
    </div>

    <div class="layout-topbar-actions">
      <div class="layout-topbar-menu flex gap-2 items-center">
        <div>
          <p-select
            [options]="languages()"
            [(ngModel)]="selectedLanguage"
            (ngModelChange)="changeLanguage()"
            optionLabel="name"
            class="w-full"
          >
            <ng-template #selectedItem let-selectedOption>
              <div class="flex items-center gap-2" *ngIf="selectedOption">
              <img
                  [src]="selectedOption.img"
                   width="18px"
                   height="18px"
                />
                <div>{{ selectedOption.name | translate }}</div>
              </div>
            </ng-template>
            <ng-template let-language #item>
              <div class="flex items-center gap-2">
                <img
                  [src]="language.img"
                  width="18px"
                  height="18px"
                />
                <div>{{ language.name | translate }}</div>
              </div>
            </ng-template>
          </p-select>
        </div>
        <p-button icon="pi pi-bell" [rounded]="true" [outlined]="true" />
      </div>
    </div>
  </div>`,
})
export class AppTopbar {
  selectedLanguage! : ILanguageOption;
  items!: MenuItem[]; 

  languages = signal<ILanguageOption[]>([
    { name: 'languages.english', code: 'en', img: 'https://hatscripts.github.io/circle-flags/flags/us.svg' },
    { name: 'languages.spanish', code: 'es', img: 'https://hatscripts.github.io/circle-flags/flags/es.svg' },
  ]);

  private _languageService = inject(LanguageService);

  constructor(public layoutService: LayoutService) {
     this.checkLanguage();
  }

  checkLanguage(){
    const code = this._languageService.currentLang();
    const found = this.languages().find((x) => x.code === code);
    console.log("found", found);
    
    this.selectedLanguage = found ?? this.languages()[0];
  }

  changeLanguage(){
    this._languageService.changeLanguage(this.selectedLanguage.code);
  }
}