# Cosign 3.1.3 help fixtures

Captured on 2026-10-05 from the official Windows amd64 release, Git commit
`11926fa5bbbbde47e88fc006b625a17769b743b2`, built on 2026-08-05.
The executable SHA-256 matches the published checksum:
`9fe59be0eca1271873ce019061335eb1ac419b7059202e797828467ddabe33be`.

`root.txt` contains `cosign --help`; `version.txt` contains `cosign version`.
Other files contain `cosign <command> --help` for all 29 commands in the existing
coverage manifest and the 11 hardware-token commands exposed by this Windows binary.
The hardware-token fixtures also verify that group-local `--no-input` is not promoted
to a tool-wide option. Line endings and trailing whitespace are normalized; help text
is otherwise retained. No signing, registry, login, or key-generation operation ran.

The tagged root parser registers `--output-file`, `--timeout` / `-t`, and
`--verbose` / `-d` through `PersistentFlags`. Root help uses the alternate
Cobra declaration layout; leaf and group help list the same three inherited flags.
Help and version are control operations. Signing keys, identity tokens, registry
credentials, and verification policies remain command-local.

Sources:
- https://github.com/sigstore/cosign/releases/tag/v3.1.3
- https://github.com/sigstore/cosign/releases/download/v3.1.3/cosign_checksums.txt
- https://github.com/sigstore/cosign/blob/v3.1.3/cmd/cosign/cli/options/root.go
