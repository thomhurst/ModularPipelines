# pnpm 12.8.1 audit evidence

Captured on Windows on 15 October 2026 with Node 24.14.1 and exact `pnpm@12.8.1`
installed in an isolated directory with lifecycle scripts disabled. Its optional
Windows native package supplies the binary; the launcher is `bin/pnpm.mjs`.

- `pnpm-help.txt`: `node <pnpm>/bin/pnpm.mjs --help`.
- `pnpm-install-help.txt`: `node <pnpm>/bin/pnpm.mjs help install`.
- `pnpm-exec-help.txt`: `node <pnpm>/bin/pnpm.mjs help exec`.

The [official CLI reference](https://pnpm.io/pnpm-cli) documents `--dir`,
`--workspace-root`, and `--ignore-workspace` as general execution controls. The
captured root help additionally identifies registry as universal and describes
reporter/context controls. It also lists workspace operations; root presence alone
does not justify promoting filters, recursion, scheduling, or streaming switches.

The tests retain the real Options section and reduce the command catalog to install
for bounded traversal. The system pnpm 11.28.4 was not used as pnpm 12 evidence.

Trailing whitespace on captured help lines is stripped; blank lines are retained.
