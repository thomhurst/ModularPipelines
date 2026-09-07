import type { PrismTheme } from 'prism-react-renderer';
import type { Config } from '@docusaurus/types';
import type * as Preset from '@docusaurus/preset-classic';
import type { PluginOptions } from '@signalwire/docusaurus-plugin-llms-txt';

const pipelineCodeTheme: PrismTheme = {
  plain: {
    color: 'var(--mp-code-ink)',
    backgroundColor: 'var(--mp-code-surface)',
  },
  styles: [
    { types: ['comment', 'prolog', 'doctype', 'cdata'], style: { color: 'var(--mp-code-comment)' } },
    { types: ['keyword', 'selector', 'tag', 'atrule'], style: { color: 'var(--mp-code-keyword)' } },
    { types: ['class-name', 'function', 'builtin', 'attr-name'], style: { color: 'var(--mp-code-type)' } },
    { types: ['string', 'char', 'number', 'boolean', 'constant', 'regex', 'attr-value'], style: { color: 'var(--mp-code-literal)' } },
    { types: ['deleted'], style: { color: 'var(--ifm-color-danger-dark)' } },
    { types: ['inserted'], style: { color: 'var(--mp-code-type)' } },
    { types: ['bold'], style: { fontWeight: 'bold' } },
    { types: ['italic'], style: { fontStyle: 'italic' } },
  ],
};

const config: Config = {
  title: 'Modular Pipelines',
  tagline: 'Strongly typed pipelines, orchestrated automatically.',
  favicon: 'img/favicon.ico',

  plugins: [
    [
      '@signalwire/docusaurus-plugin-llms-txt',
      {
        siteTitle: 'Modular Pipelines Documentation',
        siteDescription: 'Documentation for ModularPipelines - a C# framework for building modular, testable CI/CD pipelines with dependency injection support.',
        content: {
          enableMarkdownFiles: true,
          enableLlmsFullTxt: true,
        },
      } satisfies PluginOptions,
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
        alt: 'Modular Pipelines — connected modules forming an M',
        src: 'img/modular-pipelines-logo.png',
        width: 31,
        height: 31,
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
      theme: pipelineCodeTheme,
      darkTheme: pipelineCodeTheme,
    },
  } satisfies Preset.ThemeConfig,
};

export default config;
