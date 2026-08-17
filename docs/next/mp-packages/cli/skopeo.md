# skopeo CLI reference

`ModularPipelines.Skopeo` provides strongly typed access to the `skopeo` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `skopeo` executable. Install it separately and ensure `skopeo` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Skopeo
```

Resolve the service with `context.Tools.Skopeo`. For projects older than C# 14, import `ModularPipelines.Skopeo.Extensions` and use the `context.Skopeo()` extension method as a compatibility fallback.

## Module example[​](#module-example "Direct link to Module example")

Resolve the service in a module, then select a command from the table below. A runnable example is omitted when no command has complete safety metadata:

```
var skopeo = context.Tools.Skopeo;
```

## Commands[​](#commands "Direct link to Commands")

| CLI command                    | Options record                     |
| ------------------------------ | ---------------------------------- |
| `skopeo copy`                  | `SkopeoCopyOptions`                |
| `skopeo delete`                | `SkopeoDeleteOptions`              |
| `skopeo generate-sigstore-key` | `SkopeoGenerateSigstoreKeyOptions` |
| `skopeo inspect`               | `SkopeoInspectOptions`             |
| `skopeo list-tags`             | `SkopeoListTagsOptions`            |
| `skopeo login`                 | `SkopeoLoginOptions`               |
| `skopeo logout`                | `SkopeoLogoutOptions`              |
| `skopeo manifest-digest`       | `SkopeoManifestDigestOptions`      |
| `skopeo standalone-sign`       | `SkopeoStandaloneSignOptions`      |
| `skopeo standalone-verify`     | `SkopeoStandaloneVerifyOptions`    |
| `skopeo sync`                  | `SkopeoSyncOptions`                |
