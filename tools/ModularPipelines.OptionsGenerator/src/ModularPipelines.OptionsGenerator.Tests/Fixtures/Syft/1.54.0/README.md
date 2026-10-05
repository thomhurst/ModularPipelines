# Syft 1.54.0 inherited-option audit

Captured on Windows from the official `syft_1.54.0_windows_amd64.zip` release.
The archive SHA-256 was checked against the published checksum file:
`77f4b472779058e819eec9a054753a5071a996aaa40db31a290f8b256748593f`.
`version.txt` records the binary's version, commit, and build details.

Each help fixture is the output of `syft <command> --help`; `root-help.txt`
comes from `syft --help`. The eight production commands and both command groups
are covered. Only line endings and trailing whitespace were normalized.

The root command also runs a scan, so its Flags section mixes scan options with
four persistent settings: `--config/-c`, `--profile`, `--quiet/-q`, and
`--verbose/-v`. Every captured child advertises exactly these Global Flags.
Configuration files and profiles are repeated string values. Verbosity is a
count, accepting `--verbose=2`, rather than a boolean. That syntax was verified
with `syft --verbose=2 config locations --all` (exit 0).

Scan output, cataloger selection, and source settings stay command-local.
`config --load` does not apply to `config locations`; its `--all` flag stays local.
Login passwords remain secret metadata on login, not inherited settings.
Help/version control flags remain outside the shared surface. Existing command
and version API coverage is unchanged; #5687 owns the separate version audit.

Authoritative parser evidence:

- [Syft application setup](https://github.com/anchore/syft/blob/v1.54.0/cmd/syft/internal/clio_setup_config.go) enables global configuration and logging flags.
- [Syft root command](https://github.com/anchore/syft/blob/v1.54.0/cmd/syft/internal/commands/root.go) installs scan options as root-local options.
- [Pinned clio dependency](https://github.com/anchore/syft/blob/v1.54.0/go.mod) is v0.1.1.
- [clio setup](https://github.com/anchore/clio/blob/v0.1.1/setup_config.go) registers configuration and logging through `root.PersistentFlags()`.
- [clio logging](https://github.com/anchore/clio/blob/v0.1.1/logging.go) registers verbosity with `CountVarP` and quiet as a boolean.

The scraper uses an explicit audited allowlist because the root Flags section
does not distinguish persistent settings from scan-local settings. Inherited
copies are validated before removal so a changed type or alias is not discarded.
