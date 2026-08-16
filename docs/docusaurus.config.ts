import { themes as prismThemes } from 'prism-react-renderer';
import type { Config } from '@docusaurus/types';
import type * as Preset from '@docusaurus/preset-classic';

const config: Config = {
  title: 'Modular Pipelines',
  tagline: 'Strongly typed pipelines, orchestrated automatically.',
  favicon: 'img/modular-pipelines-favicon.png',

  plugins: [
    [
      'docusaurus-plugin-llms',
      {
        generateLLMsTxt: true,
        generateLLMsFullTxt: true,
        title: 'Modular Pipelines Documentation',
        description: 'Documentation for ModularPipelines - a C# framework for building modular, testable CI/CD pipelines with dependency injection support.',
      },
    ],
  ],

  themes: [
    [
      '@easyops-cn/docusaurus-search-local',
      {
        hashed: true,
        indexBlog: false,
        indexPages: false,
      },
    ],
  ],

  // Set the production url of your site here
  url: 'https://thomhurst.github.io/',
  // Set the /<baseUrl>/ pathname under which your site is served
  // For GitHub pages deployment, it is often '/<projectName>/'
  baseUrl: '/ModularPipelines',

  // GitHub pages deployment config.
  // If you aren't using GitHub pages, you don't need these.
  organizationName: 'thomhurst', // Usually your GitHub org/user name.
  projectName: 'ModularPipelines', // Usually your repo name.

  onBrokenLinks: 'throw',
  onBrokenMarkdownLinks: 'warn',

  // Even if you don't use internationalization, you can use this field to set
  // useful metadata like html lang. For example, if your site is Chinese, you
  // may want to replace "en" with "zh-Hans".
  i18n: {
    defaultLocale: 'en',
    locales: ['en'],
  },

  presets: [
    [
      'classic',
      {
        docs: {
          sidebarPath: './sidebars.ts',
        },
        theme: {
          customCss: './src/css/custom.css',
        },
      } satisfies Preset.Options,
    ],
  ],

  themeConfig: {
    image: 'img/docusaurus-social-card.jpg',
    navbar: {
      title: 'Modular Pipelines',
      logo: {
        alt: 'Modular Pipelines Logo',
        src: 'img/modular-pipelines-logo.png',
      },
      items: [
        {
          type: 'docSidebar',
          sidebarId: 'tutorialSidebar',
          position: 'left',
          label: 'Documentation',
        },
        {
          type: 'docsVersionDropdown',
          position: 'right',
        },
        {
          href: 'https://www.nuget.org/packages/ModularPipelines',
          label: 'NuGet',
          position: 'right',
        },
        {
          href: 'https://github.com/thomhurst/ModularPipelines',
          label: 'GitHub',
          position: 'right',
        },
        {
          href: 'https://github.com/sponsors/thomhurst',
          label: 'Sponsor',
          position: 'right',
        },
      ],
    },
    footer: {
      style: 'dark',
      links: [
        {
          title: 'Documentation',
          items: [
            {
              label: 'Getting started',
              to: '/docs/next/getting-started',
            },
            {
              label: 'Core concepts',
              to: '/docs/fundamentals',
            },
          ],
        },
        {
          title: 'Community',
          items: [
            {
              label: 'Stack Overflow',
              href: 'https://stackoverflow.com/questions/tagged/ModularPipelines',
            },
            {
              label: 'Sponsor the project',
              href: 'https://github.com/sponsors/thomhurst',
            },
          ],
        },
        {
          title: 'Project',
          items: [
            {
              label: 'GitHub',
              href: 'https://github.com/thomhurst/ModularPipelines',
            },
            {
              label: 'NuGet',
              href: 'https://www.nuget.org/packages/ModularPipelines',
            },
          ],
        },
      ],
      copyright: `Copyright © ${new Date().getFullYear()} Tom Longhurst. Built with Docusaurus.`,
    },
    prism: {
      additionalLanguages: ['csharp', 'powershell', 'fsharp'],
      theme: prismThemes.github,
      darkTheme: prismThemes.dracula,
    },
  } satisfies Preset.ThemeConfig,
};

export default config;
