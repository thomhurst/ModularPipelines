# Pulumi 3.267.0 help fixtures

Captured on Windows x64 from the official [Pulumi v3.267.0 release](https://github.com/pulumi/pulumi/releases/tag/v3.267.0).
The `pulumi-v3.267.0-windows-x64.zip` SHA-256 matches the release asset digest:
`296f64cfc2b86471ee6274f0b15fc8e4a880f13886fe6cb2adbd6813fe75c326`.
Only the `pulumi.exe` entry was extracted. Capture uses an isolated `PULUMI_HOME`
and `PULUMI_SKIP_UPDATE_CHECK=true`; no account login or resource operation runs.

| File | Command |
| --- | --- |
| root.txt | `pulumi --help` |
| stack.txt | `pulumi stack --help` |
| stack-ls.txt | `pulumi stack ls --help` |
| up.txt | `pulumi up --help` |
| env.txt | `pulumi env --help` |
| env-run.txt | `pulumi env run --help` |
| version.txt | `pulumi version --help` |

Help content is unchanged apart from normalizing trailing blank lines.
`pulumi version` reports `v3.267.0`.

## Scope evidence

[Root command registration](https://github.com/pulumi/pulumi/blob/v3.267.0/pkg/cmd/pulumi/pulumi.go#L451-L477)
registers thirteen public persistent flags: color, cwd, disable-integrity-checking,
emoji, fully-qualify-stack-names, logflow, logtostderr, memprofilerate,
non-interactive, otel-traces, profiling, tracing, and verbose. They apply before
subcommands, including the embedded environment commands. The same flags appear
under `Global Flags` in all six captured command/group help pages.
The [official root reference](https://www.pulumi.com/docs/iac/cli/commands/pulumi/)
is generated for the same version.

Help and the root version flag are utility controls, not generated global options.
The hidden `tracing-header` flag is absent from public help and is not introduced.
`env` belongs to the environment command group, while stack/project selectors and
resource-operation arguments remain local. Root help contains neither group-local
`--env` nor update-local `--stack`/`--config`.

Pulumi registers `emoji` with `runtime.GOOS == "darwin"` as its default. The scraper
therefore models it as an explicit boolean value on every platform, preserving
`--emoji=false` when generated on Linux/Windows and run on macOS. The other public
boolean globals retain their existing presence-flag behavior. All thirteen public
settings are scalar; aliases and value types are checked against captured help.

Version-command generation is tracked separately in #5687; this audit preserves
the existing skip policy while checking the captured version help for inheritance.
