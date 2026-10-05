# Vault 2.1.1 help fixtures

Captured from the official Windows amd64 Vault 2.1.1 binary, build d78bbbe2d2f3d289c1ec29d431071beb23668212 (2026-09-15), using `vault <command> -help`. `root.txt` uses `vault -help`. No server, login, or API operation was executed.

The release archive SHA256 is `e07a39059d7c7380d6dc776fb5bee2183cbc3344cf9387d4cf11309b83c0dce3`, verified against the [official checksums](https://releases.hashicorp.com/vault/2.1.1/vault_2.1.1_SHA256SUMS).

[Vault CLI documentation](https://developer.hashicorp.com/vault/docs/commands) distinguishes environment configuration from per-command flags. The root parser exposes control actions, not universally inherited HTTP settings. `server` and even offline snapshot inspection expose HTTP options, but `print token` does not; output options also vary by command. Keep each command's advertised flags after its command path.

The tagged [BaseCommand parser](https://github.com/hashicorp/vault/blob/v2.1.1/command/base.go) confirms that `mfa` uses StringSliceVar, `header` uses StringMapVar, and `ns` is a hidden namespace alias. Client keys are file paths, while MFA, header values, unlock keys, root-token decoding values, OTPs, and authentication operands can contain secrets. The fixtures contain help examples only.
