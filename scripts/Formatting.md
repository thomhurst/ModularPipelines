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

## Multiple .NET installations and workspace SDK selection

On machines with multiple .NET installations, the formatter can run on .NET 11
while its MSBuild workspace loads SDK 10 configuration. For a net11.0 dependency
this reports `IsAotCompatible and EnableAotAnalyzer are not supported for the
target framework`, then skips dependent projects with `Required references did
not load`. A successful build and a zero formatter exit code do not establish
semantic verification. This affects analyzer tests and can also affect core tests.
`Configuration=Release`, PATH, or selecting the dotnet host alone is insufficient.

Use the following invocation when workspace diagnostics show an older SDK. Start
it in a fresh `pwsh -NoProfile` session from the worktree root, and exit that
session afterwards so these resolver overrides do not affect other SDKs. Set
`$dotnetRoot` to the installation containing the **exact** SDK version in
`global.json`; install that SDK first if necessary, even if roll-forward would
otherwise allow another version. The example path is for the macOS installer;
on Windows it is typically `C:/Program Files/dotnet`, and Linux paths vary.

```powershell
$dotnetRoot = '/usr/local/share/dotnet'
$sdkVersion = (Get-Content ./global.json -Raw | ConvertFrom-Json).sdk.version
$sdkRoot = Join-Path $dotnetRoot "sdk/$sdkVersion"
$dotnet = Join-Path $dotnetRoot $(if ($IsWindows) { 'dotnet.exe' } else { 'dotnet' })
if (-not (Test-Path "$sdkRoot/MSBuild.dll") -or -not (Test-Path $dotnet)) {
    throw "Install SDK $sdkVersion in $dotnetRoot before formatting."
}

$env:DOTNET_ROOT = $dotnetRoot
$env:DOTNET_MSBUILD_SDK_RESOLVER_CLI_DIR = $dotnetRoot
$env:DOTNET_MSBUILD_SDK_RESOLVER_SDKS_DIR = Join-Path $sdkRoot 'Sdks'
$env:DOTNET_MSBUILD_SDK_RESOLVER_SDKS_VER = $sdkVersion
$env:MSBuildSDKsPath = $env:DOTNET_MSBUILD_SDK_RESOLVER_SDKS_DIR
$env:MSBUILD_EXE_PATH = Join-Path $sdkRoot 'MSBuild.dll'
$env:Configuration = 'Release'

$solution = 'ModularPipelines.Analyzers.slnx'
$files = @('src/ModularPipelines.Analyzers/ModularPipelines.Analyzers.Test/ModuleAuthoringAnalyzerTests.cs')
# Set an absolute path in <repo-parent>/ModularPipelines-evidence/<lock-name>.
$evidence = '/absolute/path/to/ModularPipelines-evidence/issue-1234'

& scripts/Invoke-AgentDotNet.ps1 -DotNetPath $dotnet -SingleNode `
    -LogPath "$evidence/build.log" `
    -DotNetArguments @('build', $solution, '-c', 'Release')
if ($LASTEXITCODE -ne 0) { throw "Build failed: $LASTEXITCODE" }

& scripts/Invoke-AgentDotNet.ps1 -DotNetPath $dotnet `
    -LogPath "$evidence/format-verify.log" `
    -DotNetArguments (@('format', $solution, '--no-restore', '--verify-no-changes',
        '--severity', 'info', '--verbosity', 'diagnostic', '--include') + $files)
if ($LASTEXITCODE -ne 0) { throw "Formatting verification failed: $LASTEXITCODE" }
Get-Content "$evidence/format-verify.log" |
    Select-String 'Running .*analyzers on|Required references did not load|Msbuild failed'
```

Replace the solution, files, and evidence directory for your item. Require both
`Running ... analyzers on ModularPipelines.Analyzers.Test` lines: one under
`Running Code Style analysis` and one under `Running Analyzer Reference analysis`.
Reject any SDK/AOT load error, required-reference warning, or zero selected files.
Zero **changed** files is expected for a clean verification; it does not mean no
files were analyzed.
For applying fixes, use the same environment with the scoped apply command above;
retain the separate verification pass and its diagnostics.

The investigation in [#7431](https://github.com/thomhurst/ModularPipelines/issues/7431)
retains the original failures and later successes, including contradictory results
with Release configuration and host-only overrides. On macOS arm64 with SDK
11.0.100, the full setup above runs both test passes (72 style analyzers and 340
analyzer references). A CLI-directory-only override also succeeded in this
worktree, while the SDK-directory/version pair alone failed; earlier worktrees
reported failures with CLI-directory-only setup. Use the complete combination
rather than treating one partial override's success as a reliable fix. Analyzer
counts vary with dependencies; the project and presence of both passes matter.

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

## SA1315 and captured primary-constructor parameters

The SA1315 code fix in StyleCop.Analyzers 1.3.180 can introduce a binding error
when the inherited method parameter name matches a captured primary-constructor
parameter. SDK 11.0.100's Roslyn rename API reproduces the failure independently
of StyleCop ([dotnet/roslyn#86408](https://github.com/dotnet/roslyn/issues/86408)).
For example, renaming `Log(Func<string> messageFormatter)` to use the inherited
name `formatter` makes an existing `formatter.GetLogIssueCommand()` reference
bind to the delegate instead of the captured `IBuildSystemFormatter formatter`,
causing CS1061. The returned solution has no conflict annotation.

Before applying SA1315, explicitly rename the primary-constructor parameter to
`buildSystemFormatter` and update its bound references. Review that diff and
build, then apply the scoped SA1315 fix and build again. The method parameter
can now become `formatter` without shadowing the capture. Keep SA1315 enabled;
this rename sequence is a local workaround, not an upstream fix.

The manual diagnostic fixture below builds the original source, calls the
SDK-hosted Roslyn rename API directly, applies SA1315, confirms CS1061, and
verifies that the explicit capture rename followed by SA1315 compiles without
SA1315. A successful fixture run confirms the defect and workaround; it does
not mean upstream is fixed. It is intentionally not a CI gate: once upstream
changes, re-evaluate the expected failure.

```powershell
./scripts/Test-SA1315Rename.ps1
```

The fixture pins StyleCop 1.3.180 and copies the repository's `global.json`.
It records the resolved SDK, transformed source, and logs under the shared
checkout's `ModularPipelines-evidence/issue-7464/` entry. If multiple .NET
installations are present, use `-DotNetPath` to select the installation containing
the pinned SDK and set `DOTNET_ROOT` to that installation for the command.
Treat `Required references did not load` as incomplete reproduction, even if
formatting exits zero; the fixture rejects that result.
