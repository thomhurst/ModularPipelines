# kind CLI reference

`ModularPipelines.Kind` provides strongly typed access to the `kind` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `kind` executable. Install it separately and ensure `kind` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Kind
```

Resolve the service with `context.Tools.Kind`. For projects older than C# 14, import `ModularPipelines.Kind.Extensions` and use the `context.Kind()` extension method as a compatibility fallback.

## Module example[​](#module-example "Direct link to Module example")

Resolve the service in a module, then select a command from the table below. A runnable example is omitted when no command has complete safety metadata:

```
var kind = context.Tools.Kind;
```

## Commands[​](#commands "Direct link to Commands")

| CLI command               | Options record                |
| ------------------------- | ----------------------------- |
| `kind build`              | `KindBuildOptions`            |
| `kind build node-image`   | `KindBuildNodeImageOptions`   |
| `kind create`             | `KindCreateOptions`           |
| `kind create cluster`     | `KindCreateClusterOptions`    |
| `kind delete`             | `KindDeleteOptions`           |
| `kind delete cluster`     | `KindDeleteClusterOptions`    |
| `kind delete clusters`    | `KindDeleteClustersOptions`   |
| `kind export`             | `KindExportOptions`           |
| `kind export kubeconfig`  | `KindExportKubeconfigOptions` |
| `kind export logs`        | `KindExportLogsOptions`       |
| `kind get`                | `KindGetOptions`              |
| `kind get clusters`       | `KindGetClustersOptions`      |
| `kind get kubeconfig`     | `KindGetKubeconfigOptions`    |
| `kind get nodes`          | `KindGetNodesOptions`         |
| `kind load`               | `KindLoadOptions`             |
| `kind load docker-image`  | `KindLoadDockerImageOptions`  |
| `kind load image-archive` | `KindLoadImageArchiveOptions` |
