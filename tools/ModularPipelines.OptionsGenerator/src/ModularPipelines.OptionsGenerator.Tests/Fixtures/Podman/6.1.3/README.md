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

The fixture set includes 264 help pages, including root and the three excluded
info commands. Traversal retains the prior 258 commands. The shared traversal excludes
`system renumber` and `volume reload` because their help has neither options nor
operands; their raw help is retained as audit evidence. Version command generation is
tracked separately in #5687.
