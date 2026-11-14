---
sidebar_position: 6
---

# Why Modular Pipelines?

Most pipelines are written in YAML, and most YAML pipelines are built the same way: push a change, wait for CI, read the logs, guess, and push again.

Modular Pipelines lets you write your pipeline as a normal C# project instead. You run it with `dotnet run`, on your machine or on any build agent.

```csharp
[DependsOn<BuildModule>]
[DependsOn<TestModule>]
public class PublishModule : Module<CommandResult>
{
    protected override async Task<CommandResult> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        return await context.Tools.DotNet.PublishAsync(new DotNetPublishOptions
        {
            ProjectSolution = "src/MyApp/MyApp.csproj",
            Configuration = "Release",
        }, cancellationToken: cancellationToken);
    }
}
```

Here is what that gets you.

## Run and debug it on your own machine

Your pipeline is a console app, so it runs anywhere the .NET SDK is installed, including your laptop.

- Try a change in seconds instead of waiting for a build agent.
- Set a breakpoint, step through the code, and look at the real values.
- Find out a step is broken before you push, not ten minutes after.

## Mistakes are caught before the pipeline runs

A typo in YAML fails halfway through a build. A typo in C# doesn't compile.

- Your IDE autocompletes options and flags mistakes as you type.
- Renaming or restructuring is a safe refactor, not a find-and-replace.
- [Built-in analyzers](./how-to/analyzers.md) warn about pipeline-specific mistakes, such as a missing dependency or two modules that depend on each other.

## It runs in parallel without you planning it

Each module says what it depends on. Modular Pipelines works out the order and runs everything else at the same time.

You don't arrange jobs and stages by hand, and adding a module never means reshuffling the rest. See [execution and dependencies](./how-to/execution-and-dependencies.md).

## Steps share real data

A module can return a typed result, and any module that depends on it can read that result directly.

No more writing values to files, parsing console output, or passing strings through environment variables. See [sharing data](./how-to/sharing-data.md).

## You aren't tied to one CI provider

Your build logic lives in your repository as C#, not in one vendor's YAML format. The CI configuration shrinks to a single step:

```yaml
- run: dotnet run --project build/Pipeline.csproj
```

Moving from one build system to another means rewriting that step, not your whole pipeline.

## Common tools are already wrapped

There are typed wrappers for dozens of command line tools, including .NET, Git, Docker, Kubernetes, Helm, Terraform, Azure and AWS. Options are properties you can autocomplete, not strings you have to remember.

Anything without a wrapper can still be run as a [custom command](./how-to/custom-commands.md).

## The hard parts are built in

Things you would otherwise script by hand are a line or two of configuration:

- [Retries](./how-to/retry-policy.md) and [timeouts](./how-to/timeouts.md) for flaky steps.
- [Skipping](./how-to/skipping.md) and [run conditions](./how-to/run-conditions.md) for steps that only apply sometimes.
- [Secret masking](./how-to/secrets.md), so keys and passwords stay out of your logs.
- [Result caching](./how-to/module-caching.md), so unchanged work isn't repeated.
- [Distributed mode](./distributed/overview.md), to spread one pipeline across several machines.

## You can test your pipeline

Because modules are ordinary classes, you can [write tests for them](./how-to/testing.md). Commands and file operations are faked, so a test checks what your module would do without actually doing it.

## It's the .NET you already know

There is no new language to learn. Dependency injection, configuration, options and logging work the way they do in ASP.NET Core, and any NuGet package is available to you.

Pipeline changes also go through the same pull requests, reviews and history as the rest of your code.

## It's useful beyond CI

Since a pipeline runs locally, it can do local jobs too. A common one is developer setup: replace the long "install these twelve things" guide with a pipeline that new team members run once.

## Ready to try it?

Head to [Getting Started](./getting-started.md) to create your first pipeline in a few minutes.
