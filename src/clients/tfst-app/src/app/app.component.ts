import { Component, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { CookieService } from 'ngx-cookie-service';
import { LanguageService } from './core/services/language.service';
import { COOKIE_LANG } from './core/constants/cookie-keys.constants';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, TranslateModule],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss',
})
export class AppComponent {
  title = 'tfst-app';

 private _cookie = inject(CookieService);
 private _languageService = inject(LanguageService);
 private _translate = inject(TranslateService);

  constructor() {   
    const lang = this._cookie.check(COOKIE_LANG)
      ? this._cookie.get(COOKIE_LANG)
      : this._translate.getBrowserLang() || 'en';
    this._languageService.changeLanguage(lang);
  }
}