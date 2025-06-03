import { inject, Injectable, signal } from '@angular/core';
import { TranslateService } from '@ngx-translate/core';
import { CookieService } from 'ngx-cookie-service';
import { COOKIE_LANG } from '../constants/cookie-keys.constants';

@Injectable({
  providedIn: 'root'
})
export class LanguageService {

  private translate = inject(TranslateService);
  private cookie = inject(CookieService);

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
    console.log("Lenguage actual:", this.currentLang());
    
  }
}