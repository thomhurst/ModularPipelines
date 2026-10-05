# Grype 0.120.0 help fixtures

Captured from the official Windows amd64 release on 2026-10-05. Archive SHA-256:
`024927f5caefdc05aa4f98453e269a40e9a82978a142dacaf18c67fcab378810`.
The executable reports commit `10fbc043ecbaf8436daa9efac9f1632557caf277`.
Commands: `grype --help`, `grype db --help`, `grype db status --help`,
`grype db search --help`, `grype db search vuln --help`, `grype config --help`,
and `grype version --help`.

Authoritative parser evidence:

- [Grype SetupConfig](https://github.com/anchore/grype/blob/v0.120.0/cmd/grype/cli/cli.go)
  enables clio's global config and logging flags.
- [clio persistent flag registration](https://github.com/anchore/clio/blob/v0.1.1/setup_config.go)
  passes both configurations to the root command's `PersistentFlags()`.
- [fangs config flags](https://github.com/anchore/fangs/blob/v0.1.1/config.go)
  registers repeated config files and profiles. Grype pins clio 0.1.1, which pins
  fangs 0.1.1.
- Root scanning options are added by the root command's Grype configuration;
  they do not become persistent just because they appear beside inherited flags.

The root's four persistent settings are config (-c, stringArray), profile
(stringArray), quiet (-q, Boolean), and verbose (-v, count). Group and leaf help
agree. Database output is a command-local scalar; scan output is root-local.
No credential-bearing flag is promoted: config values are file paths and profile
values are names. Config/version utility help is captured for scope evidence;
existing command generation exclusions remain unchanged (#5687 owns version APIs).

Parser probes also passed with this executable: repeated `--config=<path>` and
`--profile=<name>` flags followed by `--verbose=0 version`, and
`-q -v=0 db status --help`. The two config files defined separate selected
profiles, confirming repeatability across files. No scan or database download
was needed for these checks.
