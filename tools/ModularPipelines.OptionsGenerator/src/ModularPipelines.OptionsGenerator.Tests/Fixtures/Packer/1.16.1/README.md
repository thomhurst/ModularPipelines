# Packer 1.16.1 help fixtures

Captured from the official Windows amd64 release on 2026-10-05. The archive SHA-256
matched HashiCorp's published SHA256SUMS:
`48e9b25ecf807959a1dcb9aa9b074e6fcd08c7b7ed89803538647c5873dbc95b`.

Root: `packer --help`. Command fixtures: `packer <command> -help`.
Line endings and trailing whitespace are normalized; help content is retained.

The root help omits machine-readable output. Packer's v1.16.1 `main.go`,
`extractMachineReadable`, consumes exactly `-machine-readable` before dispatch.
`packer -machine-readable version` and `packer version -machine-readable` produce
machine-readable output. `packer -machine-readable=false version` and
`packer -color=false version` reject flags before the subcommand. A double-hyphen
machine-readable argument is not consumed by this root parser.

`build -help` advertises `-color=false`; `fmt -help` advertises `-write=false`.
These require explicit Boolean values when false, unlike presence-only flags.

Sources:
- https://releases.hashicorp.com/packer/1.16.1/
- https://github.com/hashicorp/packer/blob/v1.16.1/main.go
- https://developer.hashicorp.com/packer/docs/commands#machine-readable-output
