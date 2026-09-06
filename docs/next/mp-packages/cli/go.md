# go CLI reference

`ModularPipelines.Go` provides strongly typed access to the `go` CLI.

## Executable prerequisite[​](#executable-prerequisite "Direct link to Executable prerequisite")

This package does not install the `go` executable. Install it separately and ensure `go` is available on `PATH`.

Follow the executable's official documentation for installation instructions.

## Package installation[​](#package-installation "Direct link to Package installation")

```
dotnet add package ModularPipelines.Go
```

Resolve the service with `context.Tools.Go`. Projects using C# 13 or another .NET language can use `context.Tools.Get<ModularPipelines.Go.Services.IGo>()` instead.

## Module example[​](#module-example "Direct link to Module example")

```
using ModularPipelines;

using ModularPipelines.Go.Options;



public class RunCommandModule : Module<CommandResult>

{

    protected override async Task<CommandResult> ExecuteAsync(

        IModuleContext context,

        CancellationToken cancellationToken)

    {

        return await context.Tools.Go.VetAsync(

            new GoVetOptions(),

            cancellationToken: cancellationToken);

    }

}
```

## Commands[​](#commands "Direct link to Commands")

| CLI command       | Options record         |
| ----------------- | ---------------------- |
| `go bug`          | `GoBugOptions`         |
| `go build`        | `GoBuildOptions`       |
| `go clean`        | `GoCleanOptions`       |
| `go doc`          | `GoDocOptions`         |
| `go env`          | `GoEnvOptions`         |
| `go fix`          | `GoFixOptions`         |
| `go fmt`          | `GoFmtOptions`         |
| `go generate`     | `GoGenerateOptions`    |
| `go get`          | `GoGetOptions`         |
| `go install`      | `GoInstallOptions`     |
| `go list`         | `GoListOptions`        |
| `go mod`          | `GoModOptions`         |
| `go mod download` | `GoModDownloadOptions` |
| `go mod edit`     | `GoModEditOptions`     |
| `go mod graph`    | `GoModGraphOptions`    |
| `go mod init`     | `GoModInitOptions`     |
| `go mod tidy`     | `GoModTidyOptions`     |
| `go mod vendor`   | `GoModVendorOptions`   |
| `go mod verify`   | `GoModVerifyOptions`   |
| `go mod why`      | `GoModWhyOptions`      |
| `go run`          | `GoRunOptions`         |
| `go telemetry`    | `GoTelemetryOptions`   |
| `go test`         | `GoTestOptions`        |
| `go tool`         | `GoToolOptions`        |
| `go version`      | `GoVersionOptions`     |
| `go vet`          | `GoVetOptions`         |
| `go work`         | `GoWorkOptions`        |
| `go work edit`    | `GoWorkEditOptions`    |
| `go work init`    | `GoWorkInitOptions`    |
| `go work sync`    | `GoWorkSyncOptions`    |
| `go work use`     | `GoWorkUseOptions`     |
| `go work vendor`  | `GoWorkVendorOptions`  |
