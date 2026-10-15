# kubectl Boolean fixtures

Global option fixtures use the same verified binary: `root.txt` is `kubectl --help`,
`options.txt` is `kubectl options`, and `version.txt` is `kubectl version --help`.
`annotate.txt` is a copy of the captured `annotate-windows.txt` command help.
The `options` heading explicitly identifies settings accepted by every command.
Command `Options` sections contain local settings and replacements; they must not
be removed merely because the same switch exists globally.

Captured on Windows from the official kubectl v1.37.1 Windows amd64 binary:
https://dl.k8s.io/release/v1.37.1/bin/windows/amd64/kubectl.exe

SHA-256 (verified against the official `.sha256` sidecar):
`14b93c4916a6f37a06fbc1f46b0a1d2404c4f9741be09f00d6caf8af3b05f1d3`.

- `annotate-windows.txt`: `kubectl annotate --help`
- `apply-edit-last-applied-windows.txt`: `kubectl apply edit-last-applied --help`

The platform-default test substitutes only `--windows-line-endings=false:` to
exercise the non-Windows default; that substitution is synthetic, not a captured
Linux fixture. Both defaults must produce the same Boolean contract.

Help-only probes accepted `--recursive`, `-R`, `--recursive=true`,
`--recursive=false`, `-R=true`, and `-R=false` before
`--filename=unused.yaml --help`. No cluster operation was executed.

The short filename description and selector operator list in the annotate help
are the binary's actual output, not truncated scraper descriptions.

Trailing whitespace was removed from the captured output. In `options.txt`, the
Windows user-specific `--cache-dir` default is sanitized to `~/.kube/cache`.
The captured description remains unchanged; the scraper already removes this
platform-specific default from generated option documentation.

Additional help-only probes accepted `--recursive=` with each of `1`, `0`, `t`,
`f`, `T`, `F`, `true`, `false`, `True`, `False`, `TRUE`, and `FALSE` (exit 0).
The same invocation rejected `tRuE`, ` false`, and `false ` (exit 1), confirming
pflag's exact Boolean spellings rather than .NET's whitespace-tolerant parsing.

Set-command fixtures use the same checksum-verified v1.37.1 binary:

- `set-resources.txt`: `kubectl set resources --help`
- `set-env.txt`: `kubectl set env --help`
- `set-image.txt`: `kubectl set image --help`

The resources synopsis contains an unmatched `(` before the optional limits and
requests flags. The env help documents list, import, filename, and selector
modes absent from its abbreviated synopsis. Its validation also accepts separate
TYPE NAME targets. Keep these captured files verbatim apart from trailing
whitespace; normalize in the scraper.

The checksum-verified binary rejects `set env --local -k <directory> ENV=prod`
with `one or more resources must be specified as <resource> <name> or
<resource>/<name>` before reading the directory. Unlike image/resources, env
cannot use kustomize alone as a target in this version. The operation constraints
also follow upstream [EnvOptions.Validate/RunEnv](https://github.com/kubernetes/kubectl/blob/v0.37.1/pkg/cmd/set/set_env.go)
and [SetResourcesOptions.Validate](https://github.com/kubernetes/kubectl/blob/v0.37.1/pkg/cmd/set/set_resources.go).
