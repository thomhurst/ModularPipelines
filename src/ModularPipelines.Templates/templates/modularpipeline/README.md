# TemplatePipeline

This pipeline restores and builds the configured solution, runs its tests, and then
publishes the deployable project to `artifacts/publish`.

Edit `appsettings.json` when paths or the build configuration change. Environment
variables can override any setting, for example:

```bash
Build__Configuration=Debug dotnet run
```

The dependency chain is explicit:

```text
RestoreModule -> BuildModule -> TestModule -> PublishModule
```

Run the pipeline:

```bash
dotnet run
```

<!--#if (tests)-->
## Module tests

Run the generated TUnit test project:

```bash
dotnet run --project Tests/TemplatePipeline.Tests.csproj
```

`BuildModuleTests` uses `ModuleTester` from `ModularPipelines.Testing` to execute
`BuildModule` in isolation. It supplies build settings and a successful
`RestoreModule` result, then checks the recorded build command. The harness
intercepts commands and uses an in-memory filesystem, so the test does not run
`dotnet build` or restore the configured solution.

<!--#endif-->
If you publish the pipeline and move it away from its build machine, point the
published host at the generated pipeline project directory:

```bash
MODULAR_PIPELINES_DIRECTORY=/path/to/TemplatePipeline dotnet TemplatePipeline.dll
```
