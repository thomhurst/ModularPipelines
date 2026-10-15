# Yarn Berry 4.18.1 audit evidence

Captured on Windows on 15 October 2026 with Node 24.14.1 and the exact
`@yarnpkg/cli-dist@4.18.1` package installed in an isolated directory using
`npm install --ignore-scripts --no-audit --no-fund`. No global installation changed.

- `yarn-help.txt`: `node <cli-dist>/bin/yarn.js --help`.
- `yarn-install-help.txt`: `node <cli-dist>/bin/yarn.js install --help`.
- A read-only probe, `node <cli-dist>/bin/yarn.js --cwd <directory> config get cacheFolder`,
  succeeds. The installed bundle's `checkCwd` dispatcher consumes leading `--cwd`.
  The [upstream dispatcher](https://github.com/yarnpkg/berry/blob/master/packages/yarnpkg-cli/sources/lib.ts)
  and [BaseCommand validation](https://github.com/yarnpkg/berry/blob/master/packages/yarnpkg-cli/sources/tools/BaseCommand.ts)
  explain why it must precede the command path.
- [Official install options](https://yarnpkg.com/cli/install) describe `--immutable`,
  `--json`, and `--mode` on install, not on every Yarn command.

The root help fixture is retained for audit evidence; tests use its install help
with a reduced discovery catalog. Classic 1.22.22 is installed on the machine but
was not used to infer Berry globals. Hosted generation uses Yarn 4 project context.

Trailing whitespace on captured help lines is stripped; blank lines are retained.

The matching late-position probe (yarn config get cacheFolder --cwd <directory>)
exits 1 with an ambiguity error, confirming that placement is semantically required.
