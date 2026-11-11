# Windows parent-exit runtime reproducer

This standalone .NET 11.0.0 reproducer investigates [#6302](https://github.com/thomhurst/ModularPipelines/issues/6302).
It imports no ModularPipelines code or packages. The wrapper creates an isolated
project and SDK pin in the evidence directory, leaving the repository SDK unchanged.

On Windows with SDK 11.0.100 installed:

```powershell
& scripts/Invoke-WindowsParentExitProbe.ps1 `
    -OutputDirectory ../ModularPipelines-evidence/issue-6302/windows
```

Or dispatch the existing `.NET` workflow on the branch containing the probe:

```shell
gh workflow run dotnet.yml --ref issue-6302-windows-startup \
  -f windows-parent-exit-probe=true -f publish-packages=false -f distributed=false
```

The opt-in dispatch skips the ordinary build, test and publishing jobs. The
Windows job installs GA SDK 11.0.100 and uploads build/probe logs and `report.json`.
Use `-BuildOnly` to verify compilation on another OS. All build and execution
commands pass through the agent resource guard.

The coordinator starts a supervisor, which explicitly assigns itself to two
nested jobs. Those jobs do **not** have kill-on-close enabled. The supervisor
starts a worker, which immediately spawns a descendant before reporting readiness.
The coordinator verifies both processes are alive, then kills only the supervisor,
without managed cancellation, disposal, or process-tree traversal. With
`KillOnParentExit`, both descendants must die. The unprotected control must start
successfully and survive supervisor death. Cleanup terminates all remaining probe
processes, using recorded PIDs and start times to avoid killing reused PIDs.

The matrix tests every combination of redirected standard streams (bit 1),
restricted `InheritedHandles` (bit 2), and `CreateNewProcessGroup` (bit 4), with
`KillOnParentExit` (bit 8). Control flags are 7; protected flags are 8 through 15.
A Win32 startup failure records its native error code and message. Failure to
establish the nested-job precondition or observe process readiness fails the probe.

The framework continues using pipeline-owned kill-on-close jobs until Windows
results justify enabling the runtime API. Its fallback assigns the job **after**
`Process.Start`: a descendant spawned before assignment can escape that job.
Existing host-death tests cover already-assigned commands; they do not close this
startup race. A successful runtime probe is a prerequisite to changing the
framework and running its cancellation and background-process lifetime tests.
