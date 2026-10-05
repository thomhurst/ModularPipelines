# yq 4.54.1 help fixtures

Captured from the official Windows amd64 release with `--help`, `eval --help`,
`eval-all --help`, and `completion --help`.
Executable SHA256: b645f47ebb3a0d2fbab52998550bbd0a1706f23b5ca5f5b48e638c580bb70928.

The persistent registrations in https://github.com/mikefarah/yq/blob/v4.54.1/cmd/root.go
cover the 46 shared settings. `--version` is registered with `Flags()`, not
`PersistentFlags()`. Cobra's `--help` is a utility action. Completion is the only
utility group in this tree and is excluded from generated command coverage.
Both processing leaves print the inherited settings under `Global Flags:`.
The registrations have scalar arity, no repeated collections, and no credentials.
