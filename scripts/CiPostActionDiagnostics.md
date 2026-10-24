# Windows Cmd post-action diagnostics

The Cmd job has intermittently passed all 13 tests and then failed both
JavaScript post-actions with only `Post job cleanup.` in the ordinary logs.
[Issue #6895](https://github.com/thomhurst/ModularPipelines/issues/6895) records
two occurrences on different Windows images. No repeatable test or action defect
has been established; the original exception was not retained.

The diagnostic retry of
[run 40055647992](https://github.com/thomhurst/ModularPipelines/actions/runs/40055647992/attempts/2)
passed the tests, both post-actions and the required gate. Its worker log records
the runner's `externals/node24/bin/node.exe` launching both actions and exiting
with code 0. This establishes that diagnostic collection works, but does not
explain the original failures or establish that they were hosted-runner faults.

## Keep diagnostics enabled until an affected run is captured

On 2026-10-24, repository variables `ACTIONS_RUNNER_DEBUG` and
`ACTIONS_STEP_DEBUG` were set to `true`. These are repository settings, outside
version control. They apply to future jobs across workflows. The Cmd job records
the variable values and effective step debug flag before checkout.

[GitHub's supported configuration](https://docs.github.com/en/actions/how-tos/monitor-workflows/enable-debug-logging)
uses repository variables or secrets, rather than workflow/job `env`. A secret
with the same name overrides a variable. If the configuration is missing, restore
it from an authenticated checkout:

```powershell
gh variable set ACTIONS_RUNNER_DEBUG --body true
gh variable set ACTIONS_STEP_DEBUG --body true
gh variable list --json name,value --jq '.[] | select(.name | startswith("ACTIONS_"))'
```

Step debug logs include post-actions. Runner diagnostics are attached to the
workflow **log archive**, in `runner-diagnostic-logs`, after the job completes.
An upload step inside the Cmd job would run before the failing cleanup, so it
cannot capture that completed post-action evidence. Do not upload raw environment
variables, credentials, or unfiltered runner files as public artifacts.

## Investigate the next occurrence

1. Download the failing attempt's log archive before rerunning anything. Use an
   evidence directory as described in the issue/PR loop skill. For example, in
   PowerShell 7.4 or newer (which preserves native binary redirection):

   ```powershell
   $runId = 40055647992 # Replace with the affected run.
   $attempt = 1 # Use the failing attempt, not a later successful retry.
   $evidence = 'D:/git/ModularPipelines-evidence/issue-6895'
   New-Item -ItemType Directory -Force $evidence | Out-Null
   gh api "repos/thomhurst/ModularPipelines/actions/runs/$runId/attempts/$attempt/logs" > "$evidence/attempt-$attempt.zip"
   if ($LASTEXITCODE -ne 0) { throw 'Log download failed' }
   Expand-Archive -LiteralPath "$evidence/attempt-$attempt.zip" -DestinationPath "$evidence/attempt-$attempt" -Force
   ```

2. Extract the Cmd job's zip from `runner-diagnostic-logs`. Inspect its `Worker_`
   log around setup-dotnet and checkout cleanup. Retain the process executable,
   exit code, launch exception, timestamps, runner version and Windows image
   version. Compare those with the step logs, test result and required gate;
   distinguish a Node process crash or launch failure from an action exception.
3. Record the failing run, job and attempt links on #6895 (reopen it if needed).
   Add the relevant sanitized exception and exit code. Keep the complete archive
   in the evidence directory; do not paste raw diagnostics into an issue.
4. Fix a workflow/action defect only when the evidence identifies one. For a
   hosted-runner fault, link the matching upstream report with runner/image
   versions and the captured exception. A passing retry alone proves neither.
   Do not repeatedly rerun unchanged code, ignore post-action failures, or relax
   the required gate.

Once an affected run has been captured and its cause dispositioned on #6895,
remove these diagnostic variables if no other investigation needs them:

```powershell
gh variable delete ACTIONS_RUNNER_DEBUG
gh variable delete ACTIONS_STEP_DEBUG
```

Also check for overriding secrets and the Cmd job's diagnostic-status output.
Until then, leave collection enabled so the original failing attempt, rather than
only a successful retry, retains the evidence.
