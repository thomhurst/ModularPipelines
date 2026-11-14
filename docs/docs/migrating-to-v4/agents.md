---
title: Reference for coding agents
sidebar_position: 16
---

# Reference for coding agents

This page is for coding agents automating a consumer's migration. Use the consumer's current V3
code and the installed V4 package APIs as evidence. Do not infer the V3 API from current
documentation examples.

The replacements themselves are in the [old-to-new API lookup](./api-lookup.md). Treat them as
**symbol-scoped** transformations, not global string substitutions.

## Workflow

1. **Inventory.** List all `ModularPipelines*` package references, shared module libraries, custom
   integrations, source-generator settings, global usings, aliases, analyzer suppressions, and
   target frameworks.
2. **Upgrade packages.** Select one compatible V4 package set. Upgrade and rebuild shared projects
   so their generated metadata matches the host. Preserve the consuming repository's SDK and
   validation instructions.
3. **Symbols and entry point.** Update resolved symbols and entry points first. Keep application DI
   on `builder.Services`, and move only framework registration calls to the pipeline builder.
4. **Configuration and options.** Rewrite configuration overrides and immutable options. Preserve
   dependencies, base configuration, conditions, timeouts, retry intent, lifecycle ordering, and
   failure policy.
5. **Results and integrations.** Update result contracts, integration calls, and generated option
   properties. List the choices that need intent: nullable outputs, optional dependencies, skip
   composition, custom Polly policies, CLI option types, installer behavior, and exception handling.
6. **Build.** Build the affected pipeline and shared libraries. Resolve compiler and generator
   errors against the installed APIs. Use the compiler to resolve overloads rather than
   suppressing diagnostics.
7. **Plan.** Inspect a plan/validation run where appropriate. Planning can still construct services
   and evaluate planning-safe conditions. It is not a substitute for command and behavior tests.
8. **Test.** Test representative commands without external side effects, including exact argument
   tokens, skip/failure/cancellation behavior, cleanup, working directories, output parsing, and
   redaction. Report remaining manual decisions and validation limits before running publishing or
   deployment work.

## Guardrails

- Do not globally replace `.Value`, `.Status`, `.Get`, `.Build`, `File`, `Folder`, or `token:`.
  Resolve the receiver/type first. Keep successful-result `Value` and `ValueOrDefault` semantics
  distinct. `Value` does not reject a successful null; retain or add explicit null checks where
  required.
- Preserve `PipelineRequirement.Pass`, `Fail`, and `When` helper calls in derived requirements.
  Only their evaluation override needs migration to `EvaluateAsync`.
- Do not infer pipeline failure from the presence of any result exception. Ignored failures retain
  exceptions. Use `summary.Succeeded` for the pipeline outcome and `summary.Failures` for the
  failures that caused it.
- Review every module with more than one `WithSkipWhen` call. V3 kept only the last call; V4 ORs
  them all. Remove calls that V3 ignored, or use `WithSkipWhenAll` for AND.
- Do not replace every `Task` with `ValueTask`. Only the changed configuration callback contracts
  need that. Module execution and lifecycle hooks still use `Task`.
- Do not turn a required dependency into optional merely to make validation pass.
- Do not delete lifecycle hooks or custom policies because a fluent method disappeared.
- Do not blindly replace multiple OS attributes with multiple `[RunIf]` attributes. Preserve the
  OR/AND intent.
- Do not use regex to infer generic result types or strip nullable annotations. Inspect the
  module's declared result and its consumers.
- Do not hand-edit generated options, add obsolete aliases/shims, or invent missing properties.
  Migrate consumer calls to the actual generated contract. Generator defects belong in the generator.
- Do not suppress metadata diagnostics to force an old integration assembly to load.
- Keep a final report of modified APIs, behavioral decisions, tests run, and unverified external
  commands.

## Searches for migration candidates

Run these in the consumer repository. Treat matches as review candidates, including fully
qualified names and aliases. A match does not prove a symbol belongs to ModularPipelines.

```powershell
rg -n 'PipelineBuilderOptions|ExecutePipelineAsync|\.Build\(\)|Services\.AddModule|ConfigureServices' --glob '*.cs'
rg -n 'ModuleConfiguration|DeclareDependencies|WithRetryCount|WithRetryPolicy|WithBeforeExecute|WithAfterExecute' --glob '*.cs'
rg -n 'IPipelineGlobalHooks|IPipelineModuleHooks|IPipelineHookContext|MustAsync|ExecuteModuleAsync|ExecuteModule' --glob '*.cs'
rg -n '\.SubModule\(|\.DotNet\(\)|\.Git\(\)|ExecuteCommandLineTool|LogSettings|token:' --glob '*.cs'
rg -n 'ModuleResultType|IsSuccess|IsFailure|IsSkipped|GetModuleResultsAsync|GetFailedModuleResults|PipelineTerminated' --glob '*.cs'
rg -n 'ModularPipelines\.FileSystem\.(File|Folder)|WorkingDirectory\s*=|SkipDecision\.Of' --glob '*.cs'
rg -n 'Files\.ExistsAsync\(' --glob '*.cs'
rg -n '\b(FromHex|ToHex)\s*\(' --glob '*.cs'
rg -n 'IEventHandlerPriority|PluginRegistry|PluginTestHelper|RequestRetry|SkipDependentModules|FailPipeline|ISecretObfuscator' --glob '*.cs'
rg -n 'RunIfAll|MandatoryRunConditionAttribute|IConditionAttribute|BuildSystem\.Is[A-Z]|ArgumentPlacement|CliArgument\([^)]*Name' --glob '*.cs'
rg -n '\b(IsRunningInCI|IsRunningLocally|IsRunningIn)\s*\(' --glob '*.cs'
rg -n '\b(IZip|IXml|IYaml|IBase64|IHex|ICertificates|IDownloader|IEnvironmentVariables|IAptGet|AptGet\w*)\b' --glob '*.cs'
rg -n 'ExecutionMode|PipelineCancelledException|HttpResponseException|ModuleReferencingSelfException|FailedRequirementsException|SubModuleFailedException' --glob '*.cs'
rg -n 'ModuleRunType|ITaggedModule|IPipelineValidator|IModuleResultRepository|IModuleEstimatedTimeProvider|IFileSystemProvider|WithTimeout\(TimeSpan\.Zero' --glob '*.cs'
rg -n 'CliCommand\(|AllowMultiple\s*=|CustomSeparator|DependsOnLazy|SchedulerOptions|SecretMaskingOptions|CommandLogVerbosity\.Minimal' --glob '*.cs'
rg -n 'Information\.(BranchName|LastCommitSha|Tag|DefaultBranchName|CommitsOnBranch|PreviousCommit|Root)|RootDirectory|GetGitVersioningInformation|\.Commits\(|\.Cmd\(\)' --glob '*.cs'
rg -n 'ModularPipelinesPlugin\(|IBuildSystemDetector|IFileSystemContext|IEnvironmentDomainContext|TryGetService|WithSkipWhen' --glob '*.cs'
rg -n 'builder\.Options\.|Services\.AddRequirement|AddPipeline(Global|Module)Hooks|AddModules<|ConfigurePipelineOptions|BuildHostAsync|RootServices|RunCategories|SetLogLevel|AddPipelineFileWriter' --glob '*.cs'
rg -n 'ModuleName|ModuleDuration|ModuleStart|ModuleEnd|TotalDuration|Status\.(NotYetStarted|Processing|Successful|IgnoredFailure|UsedHistory|Retried)' --glob '*.cs'
rg -n 'RunIfAny<\w+>|RunOn\w+Only|RequirementDecision\.Of|CIEnvironment|(Windows|Linux)Requirement|Services\.Get<|\.GetService<' --glob '*.cs'
rg -n 'GenericCommandLineToolOptions|Powershell\w*Options|IncludeTimestamps|HttpLoggingType|GetLoggingHttpClient|IModuleLoggerProvider|LogToConsole|ParseTrxFile|RunCommands(OrNull)?\(' --glob '*.cs'
rg -n 'GetEnvironmentVariables?\(|Files\.Checksum|Security\.Hasher|HashType|UnZipToFolder|ZipFolder|LastWriteTimeUtc|AsSkipDecisionIfTrue|OperatingSystemHelper|ExecutionType|IModuleRegistrationEventReceiver' --glob '*.cs'
rg -n 'using ModularPipelines\.(Extensions|Models|Git\.(Models|Attributes|Extensions))|ModularPipelines\.\w+\.Generated' --glob '*.cs'
rg -n 'AzureResourceIdentifier\(|GetResourceIdentifier\(|AmazonWebServices\.Lambda|ExecutionTimeout|HttpLoggingOptions\.(None|Full)|Log(Request|Response)Body' --glob '*.cs'
```

## V4 module templates

```csharp
using ModularPipelines;

public sealed class VersionModule : Module<string>
{
    protected override void Configure(ModuleConfigurationBuilder module) => module
        .WithTimeout(TimeSpan.FromMinutes(1));

    protected override Task<string> ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
        => Task.FromResult("1.0.0");
}

[DependsOn<VersionModule>]
public sealed class PrintVersionModule : Module
{
    protected override async Task ExecuteAsync(
        IModuleContext context, CancellationToken cancellationToken)
    {
        var version = (await context.GetModule<VersionModule>()).Value;
        context.Console.WriteLine(version);
    }
}
```
