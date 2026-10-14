# npm 11.11.0 global-option audit

Captured on Windows on 15 October 2026 with npm 11.11.0 and Node 24.14.1.
The other command fixtures in this directory predate this audit and are retained.

- `npm-help.txt`: installed `npm --help`; it directs users to configuration
  documentation instead of listing shared flags.
- `npm-shared-config.json`: selected public definitions from the installed npm's
  `node_modules/@npmcli/config/lib/definitions`. Read `definitions[name].type`,
  `.short`, and `.description`; serialize constructor types by `.name`, path types
  as `Path`, and union/collection types recursively. This reads package source,
  not the user's effective configuration or credentials.
- Official versioned sources:
  [npm v11 config reference](https://docs.npmjs.com/cli/v11/using-npm/config/),
  [11.11.0 configuration documentation](https://github.com/npm/cli/blob/v11.11.0/docs/lib/content/using-npm/config.md),
  and [11.11.0 public definitions](https://github.com/npm/cli/blob/v11.11.0/workspaces/config/lib/definitions/definitions.js).
- A read-only `npm prefix --prefix <directory> --userconfig <missing-file>
  --globalconfig <missing-file>` probe returns the supplied directory.

The captured definitions cover shared execution context, configuration loading,
logging, cache, and HTTP transport. Root information flags and install/publish/script/
workspace settings remain excluded from globals. Boolean configuration renders an
explicit value; color additionally accepts `always`. CA certificates and proxy-bypass
hosts are repeatable. HTTP proxy URLs may contain credentials and need secret metadata.

Read-only prefix/config probes also confirm explicit false, zero log retention, a
fractional retry factor (1.5), and two repeated CA values on the installed parser.
