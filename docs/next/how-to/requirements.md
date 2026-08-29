# Requirements

Requirements fail a pipeline before module execution when an environment prerequisite is not satisfied. Register built-in requirements through the `Require` factory:

```
builder.AddRequirement(Require.Windows());

builder.AddRequirement(Require.EnvironmentVariable("NUGET_API_KEY"));

builder.AddRequirement(Require.Ci("Publishing can only run in CI"));
```

The platform shortcuts are `Require.Windows()`, `Require.Linux()`, `Require.MacOS()`, and `Require.WindowsAdmin()`. Use `Require.Platform(...)` for another `OSPlatform` value.

For custom asynchronous checks, implement `IPipelineRequirement` or derive from `PipelineRequirement`. Evaluation receives the pipeline cancellation token:

```
public sealed class HasDotNetSdkRequirement : PipelineRequirement

{

    public override async Task<RequirementDecision> EvaluateAsync(

        IPipelineContext context,

        CancellationToken cancellationToken)

    {

        var result = await context.Shell.RunAsync(

            "dotnet",

            ["--version"],

            cancellationToken: cancellationToken);



        return result.ExitCode == 0

            ? RequirementDecision.Passed

            : RequirementDecision.Failed(".NET SDK is not installed");

    }

}
```

For synchronous checks, return a completed task from `EvaluateAsync`:

```
public sealed class Is64BitRequirement : PipelineRequirement

{

    public override Task<RequirementDecision> EvaluateAsync(

        IPipelineContext context,

        CancellationToken cancellationToken)

        => Task.FromResult(

            Environment.Is64BitProcess

                ? RequirementDecision.Passed

                : RequirementDecision.Failed("A 64-bit process is required"));

}
```

`RequirementDecision.IsSatisfied` reports the outcome. Construct decisions with `Passed` or `Failed(reason)`; a `bool` can also convert implicitly. A bare string does not imply failure.

Requirements and run conditions receive `IPipelineContext`, giving them the same shared capability surface as global hooks. Use `IModuleContext` only inside modules.
