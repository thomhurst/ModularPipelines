---
title: "CI Example: GitHub Actions"
sidebar_position: 5
---

# CI Example: GitHub Actions

This is a complete example of running a distributed pipeline across GitHub Actions matrix runners using Redis for coordination.

## Generate the Workflow

The `ModularPipelines.GitHub` package can generate the matrix from the operating-system
capabilities declared by registered modules. Call `WriteDistributedWorkflow` after registering
the modules so regeneration reflects capability changes:

```csharp
using ModularPipelines.GitHub.Extensions;
using ModularPipelines.GitHub.PipelineWriters;

builder.AddModule<RestoreModule>();
builder.AddModule<LinuxBuildModule>();
builder.AddModule<WindowsBuildModule>();
builder.AddModule<MacBuildModule>();
builder.AddModule<AggregateResultsModule>();

builder.WriteDistributedWorkflow(new DistributedWorkflowOptions
{
    Backend = DistributedBackend.Redis,
    ExtraWorkers = 1,
    PipelineProjectPath = new("src/MyPipeline"),
});
```

The generated matrix contains a Linux master, one worker for every `linux`, `windows`, or
`macos` capability used by a registered module or its operating-system run conditions, and the
requested additional workers. It wires `INSTANCE_INDEX`, `TOTAL_INSTANCES`, `REDIS_URL`, and a
unique `RUN_IDENTIFIER` automatically. Set the repository secret
named `REDIS_URL`, or change `RedisSecretName` in the options. Commit the generated file at
`.github/workflows/modular-pipelines.yml`; regenerate it whenever module registration or
capability requirements change.

The default trigger runs on pushes to `main` and manual dispatches. Pull requests are omitted
because workflows from forks cannot access the Redis repository secret. Trusted repositories can
opt in by setting `TriggerCondition.PullRequest`; repositories accepting fork contributions should
keep secret-dependent distributed jobs disabled for those events.

The following hand-written workflow is equivalent and can be customized directly.

## Workflow File

```yaml
name: Distributed Pipeline

on:
  push:
    branches: [main]
  workflow_dispatch:

jobs:
  initialize:
    runs-on: ubuntu-latest
    outputs:
      run-identifier: ${{ steps.identifier.outputs.value }}
    steps:
      - name: Initialize coordination
        id: identifier
        shell: bash
        run: echo "value=${GITHUB_RUN_ID}-${GITHUB_RUN_ATTEMPT}" >> "$GITHUB_OUTPUT"

  pipeline:
    needs: initialize
    strategy:
      fail-fast: false
      matrix:
        instance: [0, 1, 2, 3]
        include:
          - instance: 0
            os: ubuntu-latest    # Master
          - instance: 1
            os: ubuntu-latest    # Linux worker
          - instance: 2
            os: windows-latest   # Windows worker
          - instance: 3
            os: macos-latest     # macOS worker

    runs-on: ${{ matrix.os }}

    steps:
      - name: Validate retry scope
        shell: bash
        run: |
          if [ "${{ needs.initialize.outputs.run-identifier }}" != "${GITHUB_RUN_ID}-${GITHUB_RUN_ATTEMPT}" ]; then
            echo "::error::Distributed workflows require 'Re-run all jobs'; partial retries cannot recreate the worker matrix."
            exit 1
          fi

      - uses: actions/checkout@v7.0.1

      - uses: actions/setup-dotnet@v6.0.0
        with:
          dotnet-version: "10.0.x"

      - name: Run Pipeline
        env:
          INSTANCE_INDEX: ${{ matrix.instance }}
          TOTAL_INSTANCES: 4
          REDIS_URL: ${{ secrets.REDIS_URL }}
          RUN_IDENTIFIER: ${{ needs.initialize.outputs.run-identifier }}
        run: dotnet run --project 'src/MyPipeline' -c Release
```

## Redis Setup for CI

You need a Redis instance that all runners can reach. Since GitHub Actions runners don't share a network, you need an externally hosted Redis.

### Option 1: Upstash (Recommended for CI)

[Upstash](https://upstash.com/) provides free serverless Redis with a REST API. The free tier includes 10,000 commands/day, which is sufficient for most pipeline runs.

1. Create a free Upstash database.
2. Copy the connection string (looks like `your-endpoint.upstash.io:6379,password=your-password,ssl=True`).
3. Add it as a GitHub Actions secret named `REDIS_URL`.

### Option 2: Redis Cloud

[Redis Cloud](https://redis.com/cloud/) offers a free 30MB plan. Create a database, note the public endpoint and password, and add the connection string as a secret.

### Option 3: Self-Hosted Redis

If you have a Redis instance accessible from the internet (or via a VPN), use its connection string directly.

## Pipeline Code

```csharp
using ModularPipelines;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Distributed.Extensions;
using ModularPipelines.Distributed.Redis.Extensions;
using ModularPipelines.Modules;
using Microsoft.Extensions.DependencyInjection;

using var builder = Pipeline.CreateBuilder(args);

var instanceIndex = int.Parse(
    Environment.GetEnvironmentVariable("INSTANCE_INDEX") ?? "0");
var totalInstances = int.Parse(
    Environment.GetEnvironmentVariable("TOTAL_INSTANCES") ?? "1");

builder.AddDistributedMode(o =>
{
    o.InstanceIndex = instanceIndex;
    o.TotalInstances = totalInstances;
});

builder.AddRedisDistributedCoordinator(o =>
{
    o.ConnectionString = Environment.GetEnvironmentVariable("REDIS_URL")!;
    o.RunIdentifier = Environment.GetEnvironmentVariable("RUN_IDENTIFIER");
});

builder.AddModule<RestoreModule>();
builder.AddModule<LinuxBuildModule>();
builder.AddModule<WindowsBuildModule>();
builder.AddModule<MacBuildModule>();
builder.AddModule<AggregateResultsModule>();

await builder.ExecutePipelineAsync();

public class RestoreModule : Module<string>
{
    protected override async Task<string> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        await context.Tools.DotNet.RestoreAsync(new());
        return "restored";
    }
}

[RequiresCapability("linux")]
[DependsOn<RestoreModule>]
public class LinuxBuildModule : Module<string>
{
    protected override async Task<string> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        await context.Tools.DotNet.BuildAsync(new());
        await context.Tools.DotNet.TestAsync(new());
        return "linux-ok";
    }
}

[RequiresCapability("windows")]
[DependsOn<RestoreModule>]
public class WindowsBuildModule : Module<string>
{
    protected override async Task<string> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        await context.Tools.DotNet.BuildAsync(new());
        await context.Tools.DotNet.TestAsync(new());
        return "windows-ok";
    }
}

[RequiresCapability("macos")]
[DependsOn<RestoreModule>]
public class MacBuildModule : Module<string>
{
    protected override async Task<string> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        await context.Tools.DotNet.BuildAsync(new());
        await context.Tools.DotNet.TestAsync(new());
        return "macos-ok";
    }
}

[DependsOn<LinuxBuildModule>]
[DependsOn<WindowsBuildModule>]
[DependsOn<MacBuildModule>]
public class AggregateResultsModule : Module<string>
{
    protected override async Task<string> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        var linux = await context.GetModule<LinuxBuildModule>();
        var windows = await context.GetModule<WindowsBuildModule>();
        var mac = await context.GetModule<MacBuildModule>();

        return $"All platforms passed: {linux.Value}, "
             + $"{windows.Value}, {mac.Value}";
    }
}
```

## How It Works

1. GitHub Actions starts 4 matrix jobs in parallel (instances 0-3).
2. Instance 0 (master) builds the dependency graph and enqueues `RestoreModule`.
3. Any available worker picks up `RestoreModule`, executes it, and publishes the result.
4. The master sees the restore result and enqueues the three platform-specific build modules.
5. Each build module is routed to the worker with the matching OS capability.
6. All three builds run in parallel on different runners.
7. Once all builds complete, `AggregateResultsModule` is enqueued and any available worker picks it up.
8. The master produces the pipeline summary and exits.

## Important Notes

- **Matrix jobs don't start simultaneously.** GitHub Actions may stagger runner provisioning. Workers that start before the master will wait for work to appear in the queue.
- **Runner timeout.** GitHub Actions has a 6-hour job timeout. Set `KeyExpirationSeconds` accordingly if you have very long pipelines.
- **fail-fast: false** is important — you don't want GitHub to cancel workers if the master reports an error in one module.
- **Run isolation.** The `initialize` job combines the GitHub run ID and attempt. Use
  **Re-run all jobs** after a failure so GitHub reruns the initializer and complete worker matrix
  with a fresh Redis namespace. Partial retries cannot recreate successful worker jobs, so the
  generated validation step rejects **Re-run failed jobs** before any stale coordination state is read.
- **Secrets** — store your Redis connection string as a repository or organization secret, not in code.

## Azure DevOps

The same pattern works in Azure DevOps with a matrix strategy. The run identifier auto-detects from `BUILD_SOURCEVERSION`:

```yaml
strategy:
  matrix:
    master:
      INSTANCE_INDEX: 0
      vmImage: "ubuntu-latest"
    worker-linux:
      INSTANCE_INDEX: 1
      vmImage: "ubuntu-latest"
    worker-windows:
      INSTANCE_INDEX: 2
      vmImage: "windows-latest"
```

## GitLab CI

For GitLab CI, the run identifier auto-detects from `CI_COMMIT_SHA`. Use parallel jobs or a matrix to spawn instances:

```yaml
pipeline:
  parallel:
    matrix:
      - INSTANCE_INDEX: [0, 1, 2]
  script:
    - dotnet run --project src/MyPipeline -c Release
  variables:
    TOTAL_INSTANCES: 3
    REDIS_URL: $REDIS_URL
```
