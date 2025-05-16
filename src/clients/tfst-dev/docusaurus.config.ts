import { themes as prismThemes } from "prism-react-renderer";
import type { Config } from "@docusaurus/types";
import type * as Preset from "@docusaurus/preset-classic";

const config: Config = {
  title: "The Full-Stack Team Developer Portal",
  tagline:
    "Technical docs for The Full-Stack Team contributors and integrators",
  favicon: "img/favicon.ico",

  url: "https://tfst.dev",
  baseUrl: "/",

  organizationName: "jgccon", // GitHub org
  projectName: "tfst", // GitHub repo

  onBrokenLinks: "throw",
  onBrokenMarkdownLinks: "warn",

  i18n: {
    defaultLocale: "en",
    locales: ["en", "es"],
    localeConfigs: {
      en: { label: "English" },
      es: { label: "Español", direction: "ltr" },
    },
  },

  presets: [
    [
      "classic",
      {
        docs: {
          sidebarPath: "./sidebars.ts",
          editUrl:
            "https://github.com/full-stack-team/tfst/edit/main/src/clients/tfst-dev/",
        },
        blog: {
          showReadingTime: true,
          feedOptions: {
            type: ["rss", "atom"],
            xslt: true,
          },
          editUrl:
            "https://github.com/full-stack-team/tfst/edit/main/src/clients/tfst-dev/",
          onInlineTags: "warn",
          onInlineAuthors: "warn",
          onUntruncatedBlogPosts: "warn",
        },
        theme: {
          customCss: "./src/css/custom.css",
        },
      } satisfies Preset.Options,
    ],
  ],

  themeConfig: {
    image: "img/tfst-social-card.jpg",
    navbar: {
      title: "The Full-Stack Team Docs",
      logo: {
        alt: "The Full-Stack Team Logo",
        src: "img/logo.svg",
      },
      items: [
        {
          type: "docSidebar",
          sidebarId: "tutorialSidebar",
          position: "left",
          label: "Docs",
        },
        { to: "/blog", label: "Blog", position: "left" },
        {
          type: "localeDropdown",
          position: "left",
        },
        {
          href: "https://github.com/full-stack-team/tfst",
          label: "GitHub",
          position: "right",
        },
      ],
    },
    footer: {
      style: "dark",
      links: [
        {
          title: "Docs",
          items: [
            {
              label: "Domain",
              to: "/docs/domain-model",
            },
            {
              label: "Features",
              to: "/docs/features",
            },
            {
              label: "Architecture",
              to: "/docs/architecture",
            },
            {
              label: "Contributing",
              to: "/docs/community",
            },
          ],
        },
        {
          title: "Community",
          items: [
            {
              label: "GitHub Discussions",
              href: "https://github.com/jgccon/tfst/discussions",
            },
          ],
        },
        {
          title: "More",
          items: [
            {
              label: "Roadmap",
              to: "/docs/roadmap",
            },
            {
              label: "Blog",
              to: "/blog",
            },
          ],
        },
      ],
      copyright: `© ${new Date().getFullYear()} The Full-Stack Team.`,
    },
    prism: {
      theme: prismThemes.github,
      darkTheme: prismThemes.dracula,
    },
  } satisfies Preset.ThemeConfig,
};

export default config;
