# Vault 2.2.0 HCP help fixtures

Captured with `vault hcp connect --help` and `vault hcp disconnect --help` from the official Windows amd64 Vault 2.2.0 executable, commit 2891bfd23893ebd302307050edaf03d871ea8351, built 2026-10-14T15:24:08Z. No authentication or server request was performed.

The archive SHA256 is `42264fc35dd6228dd13ade58a6e501bf1aa4910fa368b709dc9dfa23a68e537e`, verified against the [official checksums](https://releases.hashicorp.com/vault/2.2.0/vault_2.2.0_SHA256SUMS).

Unlike the usual Vault help, connect advertises its flags only through invocation examples. Passing the unknown `-bogus` flag makes its local argument parser list exactly five string flags: client-id, secret-id, organization-id, project-id, cluster-id. Disconnect rejects the same unknown flag without listing any flags; do not inherit HTTP flags from website documentation. Unknown-flag probes exited before any command action.

Trailing whitespace in the captured help was trimmed.
