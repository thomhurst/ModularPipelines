# Documentation dependency security assessments

These assessments describe the documentation site's dependency graph, not the
security of arbitrary applications using the same packages. Recheck an assessment
when its dependency versions, callers, inputs, or deployment model change.

## deepmerge: GHSA-gcqx-w48m-w2h4

Assessed on 2026-11-10 against repository commit
`5d69dec8df3da3fbde41488e84ae6adaacbc9d9a`, with Node.js 26.13.0 and Yarn 1.22.22.
Tracking issue: [#7358](https://github.com/thomhurst/ModularPipelines/issues/7358).

### Disposition

The installed `deepmerge@4.3.1` is affected, but no call to it was found in the
current documentation build or site. It is an unused transitive dependency of the
SVG build tooling. The production documentation build succeeds with the package
unavailable. This is a reachability assessment, not a claim that the package is
fixed or that the advisory is invalid.

Both lockfiles retain 4.3.1. As of the assessment date, the
[GitHub advisory](https://github.com/advisories/GHSA-gcqx-w48m-w2h4) lists no patched
version and the npm registry's latest release is 4.3.1. Dependabot alerts
[292](https://github.com/thomhurst/ModularPipelines/security/dependabot/292)
(`package-lock.json`) and
[293](https://github.com/thomhurst/ModularPipelines/security/dependabot/293)
(`yarn.lock`) remain open; this assessment does not dismiss them or introduce a
version override or an unreviewed replacement package.

### Dependency and input boundaries

`yarn why deepmerge` reports this chain:

```text
@docusaurus/preset-classic@3.10.2
  @docusaurus/plugin-svgr@3.10.2
    @svgr/webpack@8.1.0
      @svgr/plugin-svgo@8.1.0
        deepmerge@4.3.1
```

`package-lock.json` also has exactly one package declaring a `deepmerge`
dependency: `node_modules/@svgr/plugin-svgo`, with range `^4.3.1`.

Although the [SVGR 8.1.0 manifest](https://github.com/gregberge/svgr/blob/v8.1.0/packages/plugin-svgo/package.json)
declares it, the [plugin implementation](https://github.com/gregberge/svgr/blob/v8.1.0/packages/plugin-svgo/src/index.ts)
passes SVG optimization configuration to `svgo.optimize` with object spread. The
installed `@svgr/plugin-svgo/dist/index.js` imports `svgo` and `cosmiconfig`, and
does not import or call `deepmerge`. A search of installed JavaScript, CommonJS,
and ES module files found `deepmerge` references only inside deepmerge's own
package. No application source imports it either.

Docusaurus's classic preset enables its SVG loader. Its loader options and SVG
sources come from the checkout and installed dependencies. The documentation
configuration does not merge visitor request bodies or query parameters through
this package. The Pages workflow builds the checkout and publishes only
`docs/build`; it does not deploy `node_modules`, a Node.js server, or the
Docusaurus development server. Visitor input therefore has no identified route to
this dependency. This conclusion depends on the inspected code and deployment;
being a static site alone would not prove that a dependency is unreachable in
browser code.

### Reproduction and verification

From `docs/`, use a Node.js version allowed by `package.json` and Yarn Classic:

```powershell
yarn install --frozen-lockfile
yarn why deepmerge
rg --no-ignore -n --glob '*.js' --glob '*.cjs' --glob '*.mjs' deepmerge node_modules
```

The advisory's behavior was reproduced against the installed package with this
JavaScript (run in Node.js from `docs/`):

```javascript
const assert = require('node:assert/strict');
const merge = require('deepmerge');
const input = JSON.parse('{"session":{"__proto__":{"admin":true}}}');
const result = merge({ session: null }, input);
assert.equal(result.session.admin, true);
assert.equal(Object.hasOwn(result.session, 'admin'), false);
assert.equal(({}).admin, undefined);
```

The returned nested object inherits the injected property; global
`Object.prototype` is unchanged. Testing only the global prototype would miss
this behavior. See the [upstream report](https://github.com/TehShrike/deepmerge/issues/273).

To check whether the current build needs the dependency, use a fresh installation
and temporarily rename its directory. Confirm that Node cannot resolve it before
building, and restore it even if validation fails:

```powershell
Rename-Item -LiteralPath node_modules/deepmerge -NewName deepmerge-assessment-disabled
try {
  node -e 'try { require.resolve("deepmerge"); process.exit(1); } catch (error) { if (error.code !== "MODULE_NOT_FOUND") throw error; }'
  if ($LASTEXITCODE -ne 0) { throw 'deepmerge still resolves; check ancestor node_modules directories' }
  yarn build
  if ($LASTEXITCODE -ne 0) { throw 'Documentation build failed' }
} finally {
  Rename-Item -LiteralPath node_modules/deepmerge-assessment-disabled -NewName deepmerge
}
```

This checks the normal production build, including client and server compilation,
static generation, search indexing, and the generated analyzer documentation
check. It does not test every possible future configuration or optional feature.

### Reassessment triggers

Recheck both lockfiles and repeat source inspection and the build experiment when
Docusaurus, SVGR, or another dependency changes the dependency chain, when code
starts importing `deepmerge`, or when new configuration or content sources are
introduced. Reassess before adding runtime server rendering or other processing
of visitor-supplied objects. If a patched compatible release becomes available,
update both lockfiles and validate the documentation build; if the dependency is
removed upstream, verify that both lockfiles and alerts reflect its removal.
