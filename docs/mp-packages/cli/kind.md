# kind CLI reference

`ModularPipelines.Kind` provides strongly typed access to the `kind` CLI.

## Installation[​](#installation "Direct link to Installation")

```
dotnet add package ModularPipelines.Kind
```

Import `ModularPipelines.Kind.Extensions`, then resolve the service with `context.Kind()`.

## Module example[​](#module-example "Direct link to Module example")

Resolve the service in a module, then select a generated sub-domain and command from the table below:

```
using ModularPipelines.Kind.Extensions;



var kind = context.Kind();
```

## Commands[​](#commands "Direct link to Commands")

| CLI command               | Options record                |
| ------------------------- | ----------------------------- |
| `kind build node-image`   | `KindBuildNodeImageOptions`   |
| `kind create cluster`     | `KindCreateClusterOptions`    |
| `kind delete cluster`     | `KindDeleteClusterOptions`    |
| `kind delete clusters`    | `KindDeleteClustersOptions`   |
| `kind export kubeconfig`  | `KindExportKubeconfigOptions` |
| `kind export logs`        | `KindExportLogsOptions`       |
| `kind get clusters`       | `KindGetClustersOptions`      |
| `kind get kubeconfig`     | `KindGetKubeconfigOptions`    |
| `kind get nodes`          | `KindGetNodesOptions`         |
| `kind load docker-image`  | `KindLoadDockerImageOptions`  |
| `kind load image-archive` | `KindLoadImageArchiveOptions` |
