# Skopeo 1.13.3 inherited-option audit

Captured root help and all 11 command help pages from Ubuntu 24.04, matching the
workflow's `apt-get install skopeo` installation. The installed Debian package is
`1.13.3+ds1-2ubuntu0.24.04.3`; `version.txt` and `package-version.txt` record it.
The official Ubuntu image digest was
`sha256:534baea6a22c03a63003dbc8dbe78fe34bc0d7e595d9a9dc9834884ff530eb55`.
Only help/version commands were executed after installation. The capture container
was removed; no registry login, image transfer, or signing operation was performed.

Pinned parser evidence:
https://github.com/podman-container-tools/skopeo/blob/v1.13.3/cmd/skopeo/main.go

Nine public root settings use `PersistentFlags`: command timeout, debug, insecure
policy, architecture/OS/variant overrides, policy path, registries directory, and
temporary directory. `--registries.d` includes a literal dot, which must survive
in the switch while its property name becomes `RegistriesD`. Leaf help deliberately
omits the persistent settings; repeated flag names are not the inheritance evidence.

`--help` and `--version` are controls. Hidden `--registries-conf` is not part of the
public help surface. The hidden deprecated root `--tls-verify` is a **local** root
flag, distinguished with Cobra's `TraverseChildren`; it must not be inherited.
Command-local TLS verification flags support bare and explicit Boolean values.
Combined `--creds`, `--src-creds`, and `--dest-creds` values can contain passwords
and must be masked; authentication-file paths remain paths. Login's `-v` alias
means verbose and must not become the root version flag.

There are no command groups. The current 11-command/version surface is unchanged;
#5687 owns the separate version API audit.
