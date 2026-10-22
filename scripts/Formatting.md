# Applying style and analyzer fixes

Use the pinned SDK and the guarded commands in [AGENTS.md](../AGENTS.md).
The normal changed-file whitespace pass does not run style or analyzer fixes.
For those fixes, use the affected solution and select only the handwritten files
you intend to change. Build that solution first to restore its dependencies and
make project analyzers available.

## Scoped apply and verification

Run from the repository root. Replace the solution and file list for your change;
keep the list nonempty, since omitting `--include` selects the entire solution.
Do not select generated CLI output: fix its generator and regenerate instead.

```powershell
$solution = 'src/ModularPipelines.GitHub/ModularPipelines.GitHub.slnx'
$files = @('src/ModularPipelines.GitHub/GitHubAccessToken.cs')

& scripts/Invoke-AgentDotNet.ps1 -DotNetArguments (@(
  'format', $solution, '--no-restore', '--severity', 'info',
  '--exclude-diagnostics', 'IDE0130', '--include'
) + $files)
if ($LASTEXITCODE -ne 0) { throw "Format failed: $LASTEXITCODE" }

& scripts/Invoke-AgentDotNet.ps1 -DotNetArguments (@(
  'format', $solution, '--no-restore', '--verify-no-changes',
  '--severity', 'info', '--include'
) + $files)
if ($LASTEXITCODE -ne 0) { throw "Formatting verification failed: $LASTEXITCODE" }
```

The first command applies whitespace, style, and available analyzer fixes, except
for IDE0130. The second verifies formatting with no diagnostic exclusions at
info severity and above, including IDE0130. Review the diff and handle remaining
findings explicitly. A successful apply command alone does not mean verification
passed. Some analyzer diagnostics have no automatic fix, so the separate analyzer
checks still matter.

The exclusion belongs only to the local apply command. Keep CI's
`--verify-no-changes --severity info` commands and analyzer checks unchanged.
Respect the guard's existing timeout and memory limits; exits 124 and 137 are
validation limits, not successful formatting.

## IDE0130 apply-mode failure

The pinned SDK's formatter can throw
`System.NotSupportedException: Changing document properties is not supported`
when applying IDE0130 (namespace does not match folder structure). The GitHub
solution includes core files with this mismatch; a namespace fix can therefore
be selected even when no GitHub integration file was changed.

With the SDK in `global.json`, the following scoped command reproduces the
exception on `src/ModularPipelines/Modules/NonGenericSyncModule.cs`. Its declared
namespace is `ModularPipelines`, while IDE0130 expects `ModularPipelines.Modules`.
No other style/analyzer diagnostic is needed to trigger the crash.

```powershell
& scripts/Invoke-AgentDotNet.ps1 -DotNetArguments @(
  'format', 'src/ModularPipelines.GitHub/ModularPipelines.GitHub.slnx',
  '--no-restore', '--severity', 'info', '--diagnostics', 'IDE0130',
  '--include', 'src/ModularPipelines/Modules/NonGenericSyncModule.cs'
)
```

The upstream reports are [dotnet/format#1623](https://github.com/dotnet/format/issues/1623)
and [dotnet/format#2192](https://github.com/dotnet/format/issues/2192).
Excluding IDE0130 from apply mode avoids that unsupported workspace operation
without disabling its read-only verification. Fix namespace/folder mismatches
explicitly, checking public API compatibility before changing a namespace.
Do not use the exclusion to suppress CI findings.
