import { inject, Injectable, signal } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { CookieService } from 'ngx-cookie-service';
import { COOKIE_LANG } from '../constants/cookie-keys.constants';
import { PrimeNG } from 'primeng/config';
import { PRIMENG_ES } from '../../../../public/i18n/primeng/es';
import { PRIMENG_EN } from '../../../../public/i18n/primeng/en';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {

  private translate = inject(TranslateService);
  private cookie = inject(CookieService);
  private _primengConfig = inject(PrimeNG)

  currentLang = signal<string>('en'); 

  constructor() { 
    this.translate.addLangs(['en', 'es']);
  }

  changeLanguage(lang: string): void {
    if(this.translate.getLangs().includes(lang)) {
      this.cookie.set(COOKIE_LANG, lang);
    }else{
      lang = 'en';
      this.cookie.set(COOKIE_LANG, lang);
    }
    this.translate.setDefaultLang(lang);
    this.translate.use(lang);
    this.currentLang.set(lang);
    this.changePrimengLanguage(lang);
    console.log("Lenguage actual:", this.currentLang());
    
  }

  changePrimengLanguage(lang: string): void {
  switch (lang) {
    case 'es':
      this._primengConfig.setTranslation(PRIMENG_ES);
      break;
    case 'en':
      this._primengConfig.setTranslation(PRIMENG_EN);
      break;
    default:
      this._primengConfig.setTranslation(PRIMENG_EN);
  }
}

}