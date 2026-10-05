# eksctl 0.231.0 inherited-option evidence

Help was captured from the official Windows amd64 executable on 2026-10-05.
The archive SHA-256 matches the published checksum:
`ab46806ecab964e1b383c411cea51199cec07fbe28074bf1f099a2e1a24c99e3`.

- [Release and binaries](https://github.com/eksctl-io/eksctl/releases/tag/v0.231.0)
- [Root registrations](https://github.com/eksctl-io/eksctl/blob/v0.231.0/cmd/eksctl/main.go)
- [Help sections and local flag registration](https://github.com/eksctl-io/eksctl/blob/v0.231.0/pkg/ctl/cmdutils/group.go)
- [AWS and region option registration](https://github.com/eksctl-io/eksctl/blob/v0.231.0/pkg/ctl/cmdutils/cmdutils.go)

The root registers `color` (`-C`, string), `dumpLogs` (`-d`, boolean), and
`verbose` (`-v`, integer) through `PersistentFlags()`. Help is also persistent,
but remains a control action rather than a generated setting. The custom help
renderer combines persistent and inherited flags under `Common flags:`.
The root section therefore identifies the universally inherited settings;
command-local sections must not be promoted merely because their names recur.

`region`, `profile`, CloudFormation options, and output settings are registered
on local flag sets and remain on the commands that expose them. In particular,
`version --help` has a local output option but no region or profile option.
The fixtures cover root, the create group, create/get cluster leaves, and version.

A read-only smoke command accepted global flags before the subcommand:
`eksctl --color=false --dumpLogs=false --verbose=0 version` returned `0.231.0`.
The generated boolean keeps its established presence-flag contract: true emits
`--dumpLogs`; false and null omit it, using the CLI's false default.
No AWS resource operation was performed.

Version-command discovery remains owned by #5687. This audit preserves the
existing exclusion and tests its captured help only for inheritance evidence.

The captured Windows home-directory username was replaced with `contributor`;
option syntax, defaults, and descriptions otherwise remain as emitted.
Trailing blank lines and line endings were normalized.
