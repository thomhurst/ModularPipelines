# Podman root option audit

Captured on 2026-10-05 from Homebrew Podman 6.1.3 on Linux. `brew-version.json`
records the installed formula. `version.txt` records the binary version.
The Compose provider is Docker Compose 5.6.0, downloaded from its official release
with the published SHA-256 checked. Provider help is captured without rewriting;
the scraper normalizes its executable prefix.

Podman [root.go at v6.1.3](https://github.com/containers/podman/blob/v6.1.3/cmd/podman/root.go)
sets `TraverseChildren: true`. Its visible execution settings include both
persistent flags and root-local flags consumed before traversal. All 30 settings
belong before the subcommand. Root help/version are controls, not execution settings.
Hidden compatibility and profiling flags are excluded. Leaf help deliberately
omits the root settings.

`--remote`, `--syslog`, and `--transient-store` accept bare, true, and false values.
The five `stringArray` flags repeat without comma joining. Identity, TLS key,
configuration, and module values are paths, not credential contents.

`system connection add --identity` configures the new connection rather than the
active root connection. Both values must survive independently. The same applies
to the command's TLS file options. Compose provider flags remain on the compose
command; they are not Podman globals.

The fixture set includes 266 help pages, including root and the three excluded
info commands. Traversal retains 260 commands, including `podman version` and
`podman compose version`. The shared traversal excludes
`system renumber` and `volume reload` because their help has neither options nor
operands; their raw help is retained as audit evidence.

`version-help.txt` was captured on 2026-10-13 with `podman version --help` from the
official [Podman 6.1.3 Windows amd64 release](https://github.com/podman-container-tools/podman/releases/tag/v6.1.3).
The `podman-remote-release-windows_amd64.zip` SHA-256 was verified against the release
asset digest: `bb98562f5faf0df3f28bab7fb517ea9e2d807817978bfa9f68982b75e6e59c40`.
The binary reports `podman version 6.1.3`. Its `--format`/`-f` string option matches
the platform-independent [version command source](https://github.com/podman-container-tools/podman/blob/v6.1.3/cmd/podman/system/version.go).

`compose-version-help.txt` was captured on 2026-10-13 with `version --help` from
the official [Docker Compose 5.6.0 Windows x86_64 release](https://github.com/docker/compose/releases/tag/v5.6.0).
The `docker-compose-windows-x86_64.exe` SHA-256 matches the release asset digest:
`8aeaa0055e660e12cf07cd70680dc8333957fe6718528b91ec3da084eab62483`.
