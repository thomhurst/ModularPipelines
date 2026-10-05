# Argo CD 3.5.3 inherited-option audit

Captured all 166 help pages (root plus 165 generated command paths) from the official
Windows amd64 release. Only the local home-directory prefix is replaced with
`<home>`; option declarations, descriptions, and command paths are preserved.
`version.txt` records the installed binary and `commands.txt` lists the traversal.

- Release: https://github.com/argoproj/argo-cd/releases/tag/v3.5.3
- Binary: https://github.com/argoproj/argo-cd/releases/download/v3.5.3/argocd-windows-amd64.exe
- SHA-256, verified against the release checksums: `c7c5a152fe865f2262f4bb19eda66baf590cc18e0e125a902ba6942cbeee92e9`
- Pinned parser: https://github.com/argoproj/argo-cd/blob/v3.5.3/cmd/argocd/commands/root.go

The parser registers all 26 root settings with `PersistentFlags()`. Root `--help`
is Cobra's help control and is excluded. `--header` is a repeated string slice with
`-H`; authentication tokens and arbitrary header values require masking. Certificate
and key **paths** are not credential contents. Boolean persistent flags accept bare,
explicit true, and explicit false forms, including `--prompts-enabled`.

Root settings apply in the selected Cobra command's flag scope. Command-local and
command-group overrides must survive: admin Kubernetes commands redefine `--server`,
admin cluster commands redefine Redis compression settings, and `configure` defines
its own prompt setting. The `Flags` versus `Global Flags` sections establish locality;
an inherited group flag with different documentation must not be mistaken for a root
setting. Equivalent enum values can use one shared base enum without losing local
option descriptions. Unknown or changed inherited metadata is validated before
removing duplicate declarations.

The existing version API audit is tracked separately in #5687. This audit preserves
the current 165-command manifest and does not change the version skip list.
