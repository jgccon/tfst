import { ApplicationConfig, provideZoneChangeDetection, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { providePrimeNG } from 'primeng/config';
import Aura from '@primeng/themes/aura';

import { routes } from './app.routes';
import MyPreset from './mypreset';
import { provideHttpClient } from '@angular/common/http';
import {TranslateModule, TranslateLoader} from "@ngx-translate/core";
import {CookieService} from 'ngx-cookie-service';
import { HttpBackend} from '@angular/common/http';
import {MultiTranslateHttpLoader} from 'ngx-translate-multi-http-loader';


export function HttpLoaderFactory(_httpBackend: HttpBackend) {
    return new MultiTranslateHttpLoader(_httpBackend, [
      {prefix: 'i18n/common/', suffix: '.json'},
      {prefix: 'i18n/dashboard/', suffix: '.json'},
      {prefix: 'i18n/login/', suffix: '.json'},
      ]);
}

export const appConfig: ApplicationConfig = {
  providers: [
    provideAnimationsAsync(),
    CookieService,
    providePrimeNG({
      theme: {
        preset: MyPreset,
        options: {
          darkModeSelector: '.app-dark',
          cssLayer: {
            name: 'primeng',
            order: 'tailwind, primeng',
          },
        }
      },
      ripple: true
    }),
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(routes),
    provideHttpClient(),
    importProvidersFrom([TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        useFactory: HttpLoaderFactory,
        deps: [HttpBackend],
      },
    })])
    
  ],
};
