// src/utils/localization.ts
export const supportedLocales = ['en', 'es'];

export function getLocalizedPaths() {
  return supportedLocales.map(lang => ({ params: { lang } }));
}
