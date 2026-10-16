# Minikube 1.39.0 inherited-option evidence

Captured from the official Windows amd64 executable, with an isolated temporary
`MINIKUBE_HOME`. Help captures cover all 49 generated command paths and their
parent groups and discovered commands omitted by the current generator (59 help
pages), plus `minikube options` and `version --short`.
Only line endings and trailing whitespace were normalized. No cluster was started.

- Binary: https://github.com/kubernetes/minikube/releases/download/v1.39.0/minikube-windows-amd64.exe
- Published checksum: https://github.com/kubernetes/minikube/releases/download/v1.39.0/minikube-windows-amd64.exe.sha256
- Verified SHA-256: `776386465ded2cf610ae397fe302de32c44d12134b5ce7ce98ab05bd7713b360`
- Root parser: https://github.com/kubernetes/minikube/blob/v1.39.0/cmd/minikube/cmd/root.go
- Dependency versions: https://github.com/kubernetes/minikube/blob/v1.39.0/go.mod
- Logging flags: https://github.com/kubernetes/klog/blob/v2.140.0/klog.go

The root parser declares profile, bootstrapper, user, skip-audit, and rootless as
persistent flags. It initializes klog and imports the Go flag set. The dedicated
`options` command states that its flags apply to every command; root and ordinary
command help deliberately omit that table. Help is a control action, not a setting.
The remaining 20 options belong on the shared base. Command-specific driver,
format, output, and group options remain local.

klog registers `log_file_max_size` as uint64 and accepts either severity names or
integers for both threshold flags. Those shapes cannot be inferred from numeric
defaults alone. Its `log_backtrace_at` default contains a colon (`:0`), which must
not become part of the description. Boolean flags accept explicit false and bare
true; nullable generated settings must preserve both forms.

The current generator already omits the multiline `--format` declarations on
`status` and `config view`. That separate parser defect is tracked in #5800; this
audit preserves the existing command-local surface and does not promote those
options to the base class.

The verified executable accepted `--stderrthreshold=WARNING`,
`--alsologtostderrthreshold=ERROR`, and `--logtostderr=false` before
`version --short`, returning `v1.39.0`. The version help capture also covers the
command-local components, output, and short settings. Version API generation is
enabled by the generator fix for #5604.

## Operands omitted from Usage

The upstream [image command handlers](https://github.com/kubernetes/minikube/blob/v1.39.0/cmd/minikube/cmd/image.go)
consume image lists for `image pull` and `image push`, despite their operand-free
Usage lines. The [cache command handlers](https://github.com/kubernetes/minikube/blob/v1.39.0/cmd/minikube/cmd/cache.go)
likewise consume lists for `cache add` and `cache delete`. None of those handlers
enforces a minimum argument count, so the scraper supplies optional, repeatable
`Images` operands. `image tag` explicitly requires exactly two arguments:
`Source`, followed by `Target`. These source-backed corrections are scoped to
commands whose parsed Usage has no operands; future documented operands take
precedence. `cache reload` and `node list` ignore arguments and stay operand-free.

The `update-context` description spans two physical lines in the captured help.
Shared Cobra parsing preserves the first paragraph and stops before Usage or
another help section. The `cache reload` description is already complete despite
having no final period. Release identities intentionally omit the optional `v`
prefix after #6339; both spellings must resolve to the same version.
