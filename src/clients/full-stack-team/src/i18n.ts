// src/i18n.ts
import en from '../locales/en.json';
import es from '../locales/es.json';

export type Locale = 'en' | 'es';
export const defaultLocale: Locale = 'en';

const translations = { en, es };

export function t(lang: Locale, key: string): string {
  return translations[lang]?.[key] ?? key;
}