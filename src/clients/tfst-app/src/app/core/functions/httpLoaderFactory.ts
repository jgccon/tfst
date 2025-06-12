import { MultiTranslateHttpLoader } from 'ngx-translate-multi-http-loader';
import { HttpBackend } from '@angular/common/http';

export function HttpLoaderFactory(_httpBackend: HttpBackend) {
  return new MultiTranslateHttpLoader(_httpBackend, [
    { prefix: 'i18n/common/', suffix: '.json' },
    { prefix: 'i18n/dashboard/', suffix: '.json' },
    { prefix: 'i18n/login/', suffix: '.json' },
  ]);
}
