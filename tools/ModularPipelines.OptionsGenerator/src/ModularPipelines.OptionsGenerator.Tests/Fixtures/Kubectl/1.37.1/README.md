# kubectl Boolean fixtures

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

Only trailing whitespace was removed from the captured output.
