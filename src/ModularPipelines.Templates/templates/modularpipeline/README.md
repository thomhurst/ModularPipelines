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

If you publish the pipeline and move it away from its build machine, point the
published host at the generated pipeline project directory:

```bash
MODULAR_PIPELINES_DIRECTORY=/path/to/TemplatePipeline dotnet TemplatePipeline.dll
```
