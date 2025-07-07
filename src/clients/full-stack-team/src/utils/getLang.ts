// src/utils/getLang.ts
import { defaultLocale, type Locale } from '../i18n.ts';

export function getLang(Astro: any): Locale {
  return (Astro.params?.lang ?? defaultLocale) as Locale;
}
