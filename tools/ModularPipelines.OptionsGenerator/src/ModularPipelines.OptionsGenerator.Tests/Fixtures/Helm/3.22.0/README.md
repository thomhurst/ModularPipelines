# Helm persistent flag fixtures

Captured on Windows from the official Helm v3.22.0 Windows amd64 archive:
https://get.helm.sh/helm-v3.22.0-windows-amd64.zip

Archive SHA-256, verified against the official `.sha256sum` sidecar:
`899615865726d39f9b245e71e848c5bf4adc7ed33a8c43ede660facb48151b43`.

`root.txt` captures `helm --help`; `install.txt`, `template.txt`, and `version.txt`
capture the corresponding command with `--help`. Trailing whitespace was removed.
Expanded user-home paths were replaced with `~` and their path separators normalized
to `/`; other help content remains unchanged.

Helm registers settings on `cmd.PersistentFlags()` in
[cmd/helm/root.go](https://github.com/helm/helm/blob/v3.22.0/cmd/helm/root.go).
The root `Flags` section contains those settings and Cobra's local `--help`.
Child commands expose inherited settings in `Global Flags`; their own `Flags`
must retain local replacements. This audit applies to Helm, not every Cobra tool.
