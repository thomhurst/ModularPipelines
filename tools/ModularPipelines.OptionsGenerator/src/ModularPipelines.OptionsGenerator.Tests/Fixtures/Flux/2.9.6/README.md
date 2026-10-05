# Flux 2.9.6 help evidence

Captured from the official Windows amd64 `flux` 2.9.6 release with `flux [path] --help`.
The user name in the machine-specific cache default is replaced with `fixture`; trailing example whitespace is trimmed.
The production scraper omits this machine-specific default from the cache description.

- Binary archive SHA-256: `164e87a13ee8be864a96dbfe597d938cfcea60b9f309a2bd8eaad3e539fba054`.
- Source archive SHA-256: `d79ee1291c5c62dd0a405aaec9205f6039d7f47b9b3e7c2ccb8f84ef877a6abf`.
- [Release and checksums](https://github.com/fluxcd/flux2/releases/tag/v2.9.6).
- [Root registration](https://github.com/fluxcd/flux2/blob/v2.9.6/cmd/flux/main.go) binds timeout, verbose, namespace selection, Kubernetes configuration and client limits to `rootCmd.PersistentFlags()`.
- [Pinned dependency versions](https://github.com/fluxcd/flux2/blob/v2.9.6/go.mod): cli-runtime v0.36.4 and Flux runtime v0.110.3.
- [Kubernetes configuration flags](https://github.com/kubernetes/cli-runtime/blob/v0.36.4/pkg/genericclioptions/config_flags.go) establish strings, repeated string arrays, booleans, and namespace `-n`.
- [Client limits](https://github.com/fluxcd/pkg/blob/runtime/v0.110.3/runtime/client/client.go) bind integer burst and float32 QPS.
- [Get group](https://github.com/fluxcd/flux2/blob/v2.9.6/cmd/flux/get.go) and [bootstrap group](https://github.com/fluxcd/flux2/blob/v2.9.6/cmd/flux/bootstrap.go) register group-local persistent settings. These are not root globals.
- [Create receiver secret](https://github.com/fluxcd/flux2/blob/v2.9.6/cmd/flux/create_secret_receiver.go) and [trigger receiver](https://github.com/fluxcd/flux2/blob/v2.9.6/cmd/flux/trigger_receiver.go) replace `--token` with a local webhook token. Preserve local descriptions and secret masking; do not deduplicate these replacements into API bearer-token metadata.

The root help/version controls are excluded. Existing utility-command exclusions and version APIs are unchanged (#5687 owns version command work). All 23 inherited settings keep existing post-command rendering. Local receiver token properties share base storage and emit once through the existing same-scope collision resolver. Other command-group options remain local. Tests use full captured leaf help, while reducing only available-command lists to bound traversal.
