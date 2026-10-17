# Security Policy

## Supported versions

Security fixes are released for the latest major version of ModularPipelines and its integration packages.
Please upgrade to the latest release before reporting an issue.

## Reporting a vulnerability

Please do not open a public issue for a security vulnerability.

Report it privately through
[GitHub private vulnerability reporting](https://github.com/thomhurst/ModularPipelines/security/advisories/new),
or by email to the project's security contact,
[oss-security-scanner@tomlonghurst.com](mailto:oss-security-scanner@tomlonghurst.com).
Include the affected package and version, a description of the impact, and steps or a small pipeline that
reproduces the issue.

Once a fix is released, the issue is disclosed through a GitHub security advisory.

## Scope

The pipeline author is trusted: a pipeline running commands, reading environment variables, or changing files
is expected behaviour. Issues in scope include secrets leaking into output despite being registered as secrets,
argument injection from option values, path traversal, and unauthenticated access in distributed mode.
See [.oss-scanner/threat_model.md](.oss-scanner/threat_model.md) for the full threat model.
