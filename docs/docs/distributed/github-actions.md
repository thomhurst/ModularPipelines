---
title: "CI Example: GitHub Actions"
sidebar_position: 5
---

# CI Example: GitHub Actions

This is a complete example of running a distributed pipeline across GitHub Actions matrix runners using Redis for coordination.

## This repository's CI

The [.NET workflow](https://github.com/thomhurst/ModularPipelines/blob/main/.github/workflows/dotnet.yml)
runs the actual `ModularPipelines.Build` pipeline using Redis for coordination and GitHub Actions
artifacts for build outputs. It selects distributed execution only when both `REDIS_ENDPOINT` and
`REDIS_KEY` secrets are available and the event is trusted. Fork and Dependabot pull requests use
a standalone Linux pipeline, with separate Windows and macOS core build and test jobs. Manual
dispatch can also select `distributed: false` to exercise this fallback without removing secrets.

The distributed matrix uses these indices:

| Index | Runner | Responsibility |
| --- | --- | --- |
| 0 | Ubuntu | Master, Linux build, packaging, publishing, and eligible tests |
| 1 | Ubuntu | Linux test worker |
| 2 | Windows | Core build and unit tests |
| 3 | macOS | Core build and unit tests |
| 4 | Ubuntu | Linux test worker |
| 5 | Ubuntu | Linux test worker |

Every runner downloads the same framework-dependent pipeline host. Keeping this host outside
the checkout allows Windows to rebuild the build project without locking its output, and keeps
module and serialization schema identities identical across runners. Each runner restores its
own dependencies. The master publishes the registered test projects' Linux build output through
GitHub Actions artifacts. When the GitHub transport is not enabled, build outputs use the Redis
artifact store instead. A compressed TAR stores duplicate dependency contents once; workers
materialize independent files so coverage instrumentation cannot modify another test project's
assemblies. Linux tests restore that output once per process. Publishing and repository mutations
require the master's `ci-master` capability, and publishing credentials are supplied only to
instance 0. The master job alone has repository-write and identity-token permissions; worker
jobs have read-only repository access. Both jobs reuse the same execution steps.

`MODULARPIPELINES_RUN_ID` combines `GITHUB_RUN_ID` and `GITHUB_RUN_ATTEMPT`. Use **Re-run all
jobs** for a distributed retry: partial retries are rejected because they cannot recreate the
cooperating matrix. Worker capability discovery is bounded to 10 minutes, platform builds and
core tests to 75 minutes, and each runner job to 90 minutes. After a worker claims a module,
the master adds 15 minutes of result slack (`ModuleResultTimeout`) to the module's own timeout
budget, including configured retries. Redis coordination keys and fallback artifacts expire
after two hours. A failed or lost worker fails the run within these limits; it does not silently
rerun publishing or test work on another runner.

The master uploads `pipeline-report-0-ubuntu-latest-<run_attempt>`, containing `artifacts/run-report.json`.
All workflow uploads include the attempt number, so full retries preserve earlier evidence
without upload conflicts. Downloads use artifact names published by their prerequisite jobs;
standalone partial retries can therefore reuse artifacts from successful prerequisites.
Its job summary reports pipeline wall-clock duration, per-worker busy/idle time and utilization,
and total/maximum queue wait. The JSON also retains module-level timings and artifact overhead.
For a before/after comparison, run the same commit with `distributed: true` and `false`, with
package publishing disabled. Record total workflow and individual job durations from GitHub as
well: the pipeline report excludes restore and host preparation, and the standalone report also
excludes its native prebuild. CI acceptance requires successful trusted PR, main, and secretless
runs; configuration tests alone do not establish those results.

## Generate the Workflow

The `ModularPipelines.GitHub` package can generate the matrix from the operating-system
capabilities declared by registered modules. Call `AddDistributedWorkflow` after registering
the modules so regeneration reflects capability changes:

```csharp
using ModularPipelines.GitHub;

builder.AddModule<RestoreModule>();
builder.AddModule<LinuxBuildModule>();
builder.AddModule<WindowsBuildModule>();
builder.AddModule<MacBuildModule>();
builder.AddModule<AggregateResultsModule>();

builder.AddDistributedWorkflow(new DistributedWorkflowOptions
{
    Backend = DistributedBackend.Redis,
    ExtraWorkers = 1,
    PipelineProjectPath = new("src/MyPipeline"),
});
```

The generated matrix contains a Linux master, one worker for every `linux`, `windows`, or
`macos` capability used by a registered module or its operating-system run conditions, and the
requested additional workers. It wires `MODULARPIPELINES_INSTANCE_INDEX`,
`MODULARPIPELINES_TOTAL_INSTANCES`, `REDIS_URL`, and a unique
`MODULARPIPELINES_RUN_ID` automatically. Set the repository secret
named `REDIS_URL`, or change `RedisSecretName` in the options. Commit the generated file at
`.github/workflows/modular-pipelines.yml`; regenerate it whenever module registration or
capability requirements change.

The initializer publishes one framework-dependent, portable host and uploads it as
`modular-pipelines-host-<run_attempt>`. Every runner downloads those exact assemblies outside
the checkout and runs them with `--no-build --no-restore`. The project is still evaluated to
locate its executable, with `UseAppHost=false` and `OutDir` pointing at the downloaded host;
the assembly name does not need to match the project directory. Each runner installs the SDK,
keeps its own NuGet cache, and restores the project for platform-specific build/test work.
Set `DotNetRunFramework` when the project targets multiple frameworks. The host must support
every selected OS; prepare any additional native tools or runtime prerequisites on each runner.
`EnvironmentVariables` are supplied to runner restore and pipeline execution; they can use matrix
expressions such as `${{ matrix.os }}`. Supply build inputs separately through
`PublishEnvironmentVariables`, which applies only to host publishing in the non-matrix initializer.
Publish variables must use contexts available in that job and cannot reference `matrix` or pipeline job outputs.
Publishing clears the runtime identifier and disables single-file, ReadyToRun, Native AOT,
and trimming so the host stays portable and retains the metadata distributed serialization needs.
Do not use OS-conditional module registration or platform-specific host code.

Independent builds are unsafe even with the same commit and SDK: checkout line endings,
compiler inputs, generated sources, build properties, and conditional references can change
assembly module version IDs (MVIDs). The distributed schema includes user assembly build
identities so incompatible module implementations and result contracts are rejected. Sharing
the host preserves this check instead of weakening it. The
[distributed template smoke workflow](https://github.com/thomhurst/ModularPipelines/blob/main/.github/workflows/distributed-template-smoke.yml)
records independent Windows/Linux identities and their checkout/build inputs, verifies the
shared identity on both systems using the generated run command, and executes a Windows-only
module through Redis on trusted runs with coordination credentials. Secretless runs still
check the identities; they cannot verify cross-runner registration.

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
      - uses: actions/checkout@v7.0.1
        with:
          fetch-depth: 0
          persist-credentials: false
      - uses: actions/setup-dotnet@v6.0.0
        with:
          dotnet-version: "11.0.x"
      - name: Publish portable pipeline host
        shell: bash
        run: dotnet publish 'src/MyPipeline' -c Release --no-self-contained -p:UseAppHost=false -p:RuntimeIdentifier= -p:PublishSingleFile=false -p:PublishReadyToRun=false -p:PublishAot=false -p:PublishTrimmed=false -o "$RUNNER_TEMP/modular-pipelines-host"
      - uses: actions/upload-artifact@v7
        with:
          name: modular-pipelines-host-${{ github.run_attempt }}
          path: ${{ runner.temp }}/modular-pipelines-host
          if-no-files-found: error
          retention-days: 1
          include-hidden-files: true

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
        with:
          fetch-depth: 0
          persist-credentials: false

      - uses: actions/setup-dotnet@v6.0.0
        with:
          dotnet-version: "11.0.x"

      - uses: actions/download-artifact@v8
        with:
          name: modular-pipelines-host-${{ github.run_attempt }}
          path: ${{ runner.temp }}/modular-pipelines-host

      - name: Restore runner dependencies
        shell: bash
        run: dotnet restore 'src/MyPipeline'

      - name: Run Pipeline
        shell: bash
        env:
          MODULARPIPELINES_INSTANCE_INDEX: ${{ matrix.instance }}
          MODULARPIPELINES_TOTAL_INSTANCES: 4
          REDIS_URL: ${{ secrets.REDIS_URL }}
          MODULARPIPELINES_RUN_ID: ${{ needs.initialize.outputs.run-identifier }}
        run: dotnet run --project 'src/MyPipeline' -c Release --no-build --no-restore -p:UseAppHost=false --property:OutDir="$RUNNER_TEMP/modular-pipelines-host/"
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
using ModularPipelines.Distributed;
using ModularPipelines.Distributed.Redis;
using Microsoft.Extensions.DependencyInjection;

var builder = Pipeline.CreateBuilder(args);

builder.AddDistributedMode();

builder.AddRedisDistributedCoordinator(o => o with
{
    ConnectionString = Environment.GetEnvironmentVariable("REDIS_URL")!,
});

builder.AddModule<RestoreModule>();
builder.AddModule<LinuxBuildModule>();
builder.AddModule<WindowsBuildModule>();
builder.AddModule<MacBuildModule>();
builder.AddModule<AggregateResultsModule>();

await builder.RunAsync();

public class RestoreModule : Module<string>
{
    protected override async Task<string> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        await context.Tools.DotNet.RestoreAsync(new());
        return "restored";
    }
}

[RequiresCapability(Capability.Names.Linux)]
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

[RequiresCapability(Capability.Names.Windows)]
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

[RequiresCapability(Capability.Names.MacOS)]
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

1. The initializer publishes one portable host, then GitHub Actions starts 4 matrix jobs in parallel (instances 0-3). Each downloads the host and restores its local dependencies.
2. Instance 0 (master) builds the dependency graph and enqueues `RestoreModule`.
3. Any available worker picks up `RestoreModule`, executes it, and publishes the result.
4. The master sees the restore result and enqueues the three platform-specific build modules.
5. Each build module is routed to the worker with the matching OS capability.
6. All three builds run in parallel on different runners.
7. Once all builds complete, `AggregateResultsModule` is enqueued and any available worker picks it up.
8. The master produces the pipeline summary and exits.

## Important Notes

- **Matrix jobs don't start simultaneously.** GitHub Actions may stagger runner provisioning. Workers that start before the master will wait for work to appear in the queue.
- **Runner timeout.** GitHub Actions has a 6-hour job timeout. Set `RedisOptions.TimeToLive` accordingly if you have very long pipelines. Unnamed run options must exceed `DistributedOptions.ModuleResultTimeout` and `DistributedOptions.MasterTimeout`; coordinator creation also requires the TTL to exceed `DistributedOptions.WorkerTimeout`. Named module-cache options require a positive TTL independently of these timeouts.
- **fail-fast: false** is important — you don't want GitHub to cancel workers if the master reports an error in one module.
- **Run isolation.** The `initialize` job combines the GitHub run ID and attempt. Use
  **Re-run all jobs** after a failure so GitHub reruns the initializer and complete worker matrix
  with a fresh Redis namespace. Partial retries cannot recreate successful worker jobs, so the
  generated validation step rejects **Re-run failed jobs** before any stale coordination state is read.
- **Secrets** — store your Redis connection string as a repository or organization secret, not in code.

## Azure DevOps

The same pattern works in Azure DevOps with a matrix strategy. Export one shared identifier explicitly;
combining the build and stage attempt keeps full stage retries isolated:

```yaml
variables:
  MODULARPIPELINES_RUN_ID: "$(Build.BuildId)-$(System.StageAttempt)"
  MODULARPIPELINES_TOTAL_INSTANCES: 3

strategy:
  matrix:
    master:
      MODULARPIPELINES_INSTANCE_INDEX: 0
      vmImage: "ubuntu-latest"
    worker-linux:
      MODULARPIPELINES_INSTANCE_INDEX: 1
      vmImage: "ubuntu-latest"
    worker-windows:
      MODULARPIPELINES_INSTANCE_INDEX: 2
      vmImage: "windows-latest"
```

## GitLab CI

For GitLab CI, export `CI_PIPELINE_ID` explicitly as the shared identifier. Start a new pipeline rather
than retrying only part of the matrix when coordination state must be discarded:

```yaml
pipeline:
  parallel:
    matrix:
      - MODULARPIPELINES_INSTANCE_INDEX: [0, 1, 2]
  script:
    - dotnet run --project src/MyPipeline -c Release
  variables:
    MODULARPIPELINES_TOTAL_INSTANCES: 3
    REDIS_URL: $REDIS_URL
    MODULARPIPELINES_RUN_ID: $CI_PIPELINE_ID
```
