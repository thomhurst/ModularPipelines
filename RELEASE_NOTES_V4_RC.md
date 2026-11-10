# Try the ModularPipelines 4.0 release candidate

This prerelease is for testing upgrades in real consumer pipelines before 4.0.0.
Please send upgrade feedback: installation failures, migration steps that are
unclear, changed behavior, and performance regressions are all useful.

Install matching `4.0.0-rc.N` versions of every ModularPipelines package your
pipeline uses. Select the exact RC version shown on this release, enable
prerelease packages in your package manager, and use the .NET SDK pinned in
[`global.json`](https://github.com/thomhurst/ModularPipelines/blob/main/global.json)
or a compatible newer SDK. The initial release candidate requires .NET 11.

Follow the [v3 to v4 migration guide](https://modularpipelines.tomlonghurst.dev/docs/migrating-to-v4)
before running your pipeline. Include the RC version, SDK version, operating
system, a minimal reproduction, and expected versus actual behavior when
[opening a feedback issue](https://github.com/thomhurst/ModularPipelines/issues/new).
Link the [4.0 release checklist](https://github.com/thomhurst/ModularPipelines/issues/3997)
so feedback can be triaged as a release blocker or a follow-up before GA.

This release candidate does not consume the stable `v4.0.0` tag and is not the
latest stable GitHub release. Prerelease packages can change before GA.
