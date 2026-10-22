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

## C# projects with F# references

The pinned formatter loads F# projects into its Roslyn workspace, but those
projects do not support Roslyn compilations. Its analyzer runner checks every
project reference recursively and skips a C# project when an F# dependency
returns no compilation. The resulting `Required references did not load` warning
can appear after a successful build or restore, and formatting still exits zero.

[test/Directory.Build.targets](../test/Directory.Build.targets) supplies the built
F# assembly as a metadata reference during C# design-time builds. Ordinary builds
keep the project reference, including F# runtime and documentation coverage.
Build the affected solution before semantic formatting. The formatter defaults
to `Debug`; if the F# project was built in another configuration, set the
`Configuration` environment variable to that configuration for formatting, then
restore its previous value. The target preserves project configuration mappings
when MSBuild supplies them. The pinned formatter does not forward the solution's
configuration mappings, so check the build output for the actual F# configuration.
For example, when a `Release` solution maps its F# dependency to `Debug`, format
the C# project with `Configuration=Debug`, or build the dependency in `Release`
before formatting with `Configuration=Release`.

Use `--verbosity diagnostic` to confirm that the selected C# project's code-style
and analyzer-reference passes say `Running ... analyzers on <project>`. Treat
`Required references did not load`, `Msbuild failed`, or zero selected files as
incomplete validation, regardless of the exit code. A missing F# assembly now
also produces an explicit instruction to build the matching configuration.

The CI regression check builds and runs a small C#/F# fixture, verifies that
semantic formatting detects an unused import (IDE0005) and an instance member
that can be static (CA1822), applies both fixes, then verifies the result. It also
checks that both the code-style and analyzer-reference passes run on the C#
project. A separate design-time MSBuild check verifies solution configuration
mappings, and an ordering-only F# reference must neither become a compiler
reference nor require built output. Run it locally with the pinned SDK:

```powershell
./scripts/Test-FSharpFormatting.ps1
```

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
