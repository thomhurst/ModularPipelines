# Focused command validation

Use the pinned SDK on `PATH` and run from the worktree root. Set `$logPath` to a
file in the current issue's evidence directory, for example
`D:/git/ModularPipelines-evidence/issue-7107/command-tests.log`. Build once, then
run the command fixtures without rebuilding so the guard measures test execution
separately from compilation:

```powershell
& scripts/Invoke-AgentDotNet.ps1 -SingleNode `
  -DotNetArguments @('build', 'ModularPipelines.Tests.slnf', '-c', 'Release')
if ($LASTEXITCODE -ne 0) { throw "Build failed: $LASTEXITCODE" }

& scripts/Invoke-AgentDotNet.ps1 `
  -LogPath $logPath `
  -DotNetArguments @(
    'run', '--project', 'test/ModularPipelines.UnitTests/ModularPipelines.UnitTests.csproj',
    '--framework', 'net11.0', '-c', 'Release', '--no-build', '--',
    '--treenode-filter', '/*/*/(CommandTests)|(CommandLoggerTests)|(CommandInputAndExitCodeTests)/*'
  )
if ($LASTEXITCODE -ne 0) { throw "Command tests failed: $LASTEXITCODE" }
```

The log must contain a test summary with the executed count; a successful build
alone does not validate the command behavior. Follow the validation-limit rules
in [AGENTS.md](../AGENTS.md) if the guard exits 124 or 137.

These fixtures share `ProcessFixtureParallelLimit`, including the logging matrix
and standard-input/exit-code tests. Each test can create pipeline services and
launch PowerShell or .NET children, so the shared limit bounds overlap across
classes. Independent per-class budgets would still allow their children to
compete for the same process-tree memory budget.

## Windows memory regression evidence

On 2026-10-24, the invocation above was measured with SDK
`11.0.100-rc.1.26425.128`, TUnit 1.76.0, and 22 logical processors. The guard's
default 2048 MB limit and 600-second timeout were used for both runs.

| Fixtures | Sampled peak process-tree MB | Result |
| --- | ---: | --- |
| Before this fix, based on `4360d08b61` | 2270.53 | Guard exit 137; no test summary |
| Shared two-test budget across all three classes | 597.31 | 162 passed, 4 platform skips, 0 failed; 166 total |

At the failing peak, 27 PowerShell processes accounted for 1520.92 MB, while the
test executable used 246.80 MB. The logging matrix starts 32 PowerShell commands;
without the shared limiter it overlaps with the other command fixtures. This
identifies concurrent child processes during execution as the source of the
peak, rather than test discovery. The successful run took about 75 seconds.

The measurements are sampled working-set totals for the guarded descendants,
including its launcher and console hosts; they exclude the guard and sampler.
They are regression evidence, not portable memory assertions. The original
2114 MB failure, reproduction logs, and process breakdown are recorded in
[#7107](https://github.com/thomhurst/ModularPipelines/issues/7107).
