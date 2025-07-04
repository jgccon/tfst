// @ts-check
import { defineConfig } from 'astro/config';
import sitemap from '@astrojs/sitemap';

import tailwindcss from '@tailwindcss/vite';


export default defineConfig({
  site: 'https://full-stack.team',

  integrations: [
        sitemap({
            i18n: {
                defaultLocale: 'en', // All urls that don't contain `es` or `fr` after `https://full-stack.team/` will be treated as default locale, i.e. `en`
                locales: {
                en: 'en-US', // The `defaultLocale` value must present in `locales` keys
                es: 'es-ES',
                fr: 'fr-CA',
                },
            },
            xslURL: '/sitemap.xsl',
            filenameBase: 'the-full-stack-team-sitemap'
        }),
    ],
  redirects: {
    '/': '/en/',         // redirige raíz al inglés
  },
  vite: {
    plugins: [tailwindcss()],
  },
});