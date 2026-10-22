# Documentation dependency audit

The site uses Yarn Classic 1.22.22 in the GitHub Pages workflows. Use a Node
version allowed by `package.json`, then run `yarn install --frozen-lockfile` and
`yarn build` from this directory. `package-lock.json` is also maintained for npm
consumers and dependency alerts; after changing `package.json`, regenerate it
with `npm install --package-lock-only --ignore-scripts --no-audit --no-fund`, then
run `yarn install` to normalize Yarn's lockfile before the frozen validation.
Do not edit either lockfile's resolved entries by hand.

## Direct peers

- `@types/react` matches the React 19 runtime and satisfies `@mdx-js/react`.
- `@docusaurus/theme-common` is the peer used by local search.
- `@docusaurus/plugin-content-docs` is the peer used by theme-common and is also
  the docs plugin enabled by preset-classic.

Keep the Docusaurus packages at the same version. These packages are already
used by the site; declaring them directly makes their peer requirements explicit.

## Compatible transitive versions

The former blanket resolutions for AJV, picomatch, and http-proxy-middleware
crossed major versions. Let the consumers select their supported ranges:

| Consumer | Requested dependency | Audited resolution |
| --- | --- | --- |
| file-loader / null-loader / url-loader → schema-utils 3 → ajv-keywords 3 | AJV 6 | 6.15.0 |
| schema-utils 4 / ajv-formats 2 | AJV 8 | 8.20.0 |
| chokidar (anymatch/readdirp), micromatch, jest-util | picomatch 2 | 2.3.2 |
| webpack-dev-server 5 | http-proxy-middleware 2 | 2.0.10 |

AJV 8 with ajv-keywords 3 crashes in `_formatLimit.js` during file-loader schema
validation. In addition to production builds, check that file-loader's own
schema-utils accepts valid options and throws `ValidationError` for invalid
options when updating this graph. Proxy middleware must retain the API expected
by webpack-dev-server 5 rather than being forced to its latest major.

## Intentional install warnings (audited 2026-10-22)

Yarn warns when a resolution is outside the consumer's declared range
([Yarn documentation](https://classic.yarnpkg.com/en/docs/selective-version-resolutions/)).
The remaining exceptions are intentional:

- **tinypool 2.2.0**, **postcss-selector-parser 7.1.7**, and **qs 6.16.1**
  retain the patched versions introduced by
  [the security audit in #6873](https://github.com/thomhurst/ModularPipelines/pull/6873).
  Their consumers request older ranges. Docusaurus's worker-pool interface,
  CSS processing, and production build were validated there; keep these
  exceptions until upstream ranges admit the patched versions. The security pin
  for **http-cache-semantics 4.3.0** is retained as well.
  **js-yaml 5.5.0** preserves the later upgrade in [#6879](https://github.com/thomhurst/ModularPipelines/pull/6879);
  its consumers still request 4.x, so this pin also produces an intentional warning.
- **serialize-javascript 7.1.3**, requested as 6.x by copy-webpack-plugin,
  css-minimizer-webpack-plugin, and terser-webpack-plugin. The older versions
  are affected by [code injection](https://github.com/advisories/GHSA-5c6j-r48x-rmvq)
  and [CPU exhaustion](https://github.com/advisories/GHSA-qj8w-gfj5-8c6v).
  Version 7 retains the CommonJS serialization function these plugins call;
  its Node >=20 requirement is covered by this site's Node engine range.
  Keep the override and resolution until the consumers support a patched version.
- **uuid 14.0.3**, requested as 8.x by sockjs. The older version is affected by
  [missing buffer bounds checks](https://github.com/advisories/GHSA-w5hq-g745-h8pq).
  Sockjs only calls `require('uuid').v4()` without arguments. That call works on
  the supported Node versions, which support requiring this synchronous ES module.
  Keep the override and resolution until sockjs declares a patched version.
- **react-loadable peer** from react-loadable-ssr-addon-v5-slorber 1.0.3.
  [Docusaurus core 3.10.2](https://registry.npmjs.org/@docusaurus/core/3.10.2)
  supplies `react-loadable` as the alias `npm:@docusaurus/react-loadable@6.0.0`.
  Yarn Classic reports the peer as unmet despite the installed alias. The addon
  and Docusaurus resolve that same alias at runtime. Do not install the original
  unmaintained react-loadable package alongside Docusaurus's fork.
- **search-insights and @algolia/client-search peers** from Algolia autocomplete.
  [Preset-classic 3.10.2](https://registry.npmjs.org/@docusaurus/preset-classic/3.10.2)
  depends on theme-search-algolia, but only enables that theme when
  `themeConfig.algolia` is configured (see its published `lib/index.js`). This
  site enables `@easyops-cn/docusaurus-search-local` instead. Leave these unused
  peers out of the direct dependencies. Revisit them if Algolia search is enabled
  or the preset changes; local search indexing is verified by `yarn build`.
- **Mixed lockfiles**: Yarn warns about the checked-in npm lockfile. Both are
  retained and regenerated for the workflows above; use one package manager
  per install rather than mixing their installed dependency trees.
- **DEP0169**: Yarn Classic uses Node's deprecated `url.parse()` API. This is a
  package-manager warning, separate from the site's dependency peer requirements.

The broader dependency security remediation shipped in
[#6873](https://github.com/thomhurst/ModularPipelines/pull/6873); the remaining
upstream braces advisory is tracked by
[#6872](https://github.com/thomhurst/ModularPipelines/issues/6872).
This audit preserves those security fixes while restoring compatible patched
versions of picomatch and http-proxy-middleware. Re-check the advisories and
consumer ranges whenever updating or removing an exception.
