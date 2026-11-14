---
title: Integration packages
sidebar_position: 10
---

# Integration packages

Every integration is reached through `context.Tools` and has regenerated options. See
[Tools and commands](./commands.md) and [CLI options](./cli-options.md). This page covers the
packages with changes beyond that.

## Namespace changes

Several handwritten integrations flatten their options, models, attributes, and extension methods
into the package root namespace. Replace the old imports. Leaving an obsolete `using` in place
causes CS0246. Each package's own migration section covers its other API changes.

| Package migration guide | Old namespaces | V4 namespace |
| --- | --- | --- |
| [Email](../mp-packages/email.md#v4-migration) | `ModularPipelines.Email.Options`, `.Extensions` | `ModularPipelines.Email` |
| [FTP](../mp-packages/ftp.md#v4-migration) | `ModularPipelines.Ftp.Options`, `.Extensions` | `ModularPipelines.Ftp` |
| [Slack](../mp-packages/slack.md#v4-migration) | `ModularPipelines.Slack.Options`, `.Extensions` | `ModularPipelines.Slack` |
| [Microsoft Teams](../mp-packages/microsoft-teams.md#v4-migration) | `ModularPipelines.MicrosoftTeams.Options`, `.Models`, `.Extensions` | `ModularPipelines.MicrosoftTeams` |
| [GitHub](../mp-packages/github.md#v4-migration) | `ModularPipelines.GitHub.Attributes`, `.PipelineWriters`, handwritten `.Options` / `.Extensions` types | `ModularPipelines.GitHub` |
| [TeamCity](../mp-packages/teamcity.md#v4-migration) | `ModularPipelines.TeamCity.Extensions` | `ModularPipelines.TeamCity` |
| [Azure Pipelines](../mp-packages/azure-pipelines.md#v4-migration) | `ModularPipelines.Azure.Pipelines.Extensions` | `ModularPipelines.Azure.Pipelines` |
| [Git](#git) | `ModularPipelines.Git.Models`, `.Attributes`, `.Extensions` | `ModularPipelines.Git` |

## Git

### Namespaces and types

- Repository models, branch attributes, and registration extensions now use `ModularPipelines.Git`.
  Command options stay in `ModularPipelines.Git.Options`.
- `GitCommands`, `GitCommandRunner`, `GitCommitMapper`, and `GitCommandService` are internal.
  Register the integration with `RegisterGitContext`, and consume or replace the public interfaces
  (`IGitCommands`, `IGitCommandRunner`, and `IGitCommitMapper`) instead of constructing or
  subclassing the implementation types.

### Repository information is async

In V3 these were synchronous properties. V4 loads repository information once, asynchronously, and
caches it.

```csharp
// V3
var branch = context.Git().Information.BranchName;

// V4
var git = await context.Tools.Git.Information.GetRequiredInfoAsync(cancellationToken);
var branch = git.BranchName ?? "detached";
```

This covers `BranchName`, `LastCommitSha`, `Tag`, `DefaultBranchName`, `CommitsOnBranch`,
`PreviousCommit`, and `Root`. `Git().RootDirectory` becomes the same info object's `Root`.

Choose the method by whether your pipeline can run outside a Git repository:

| Method | When Git information is unavailable | Use when |
| --- | --- | --- |
| `GetRequiredInfoAsync` | Throws `InvalidOperationException` | The pipeline always runs in a repository |
| `GetInfoAsync` | Returns `null` | The pipeline must also work without Git |

"Unavailable" means, for example, outside a repository or without `git` installed.

Every `GitRepositoryInfo` property except `Root` is nullable. For example, `Tag` is `null` when no
tag exists. Handle missing values explicitly.

### Commands are grouped

| V3 API | V4 API |
| --- | --- |
| `Git().Commands.Commit(...)`, `Tag(...)`, `Checkout(...)` | `Tools.Git.Commands.Branches.CommitAsync(...)`, `TagAsync(...)`, `CheckoutAsync(...)` |
| `Git().Commands.Push(...)`, `Fetch(...)` | `Tools.Git.Commands.Remotes.PushAsync(...)`, `FetchAsync(...)` |
| `Git().Commands.Add(...)`, `Status(...)` | `Tools.Git.Commands.WorkingTree.AddAsync(...)`, `StatusAsync(...)` |
| `Git().Commands.Commits(...)` | `Tools.Git.Commands.History.CommitsAsync(...)` |
| `Git().Versioning.GetGitVersioningInformation()` | `Tools.Git.Versioning.GetVersioningInformationAsync(cancellationToken)` |
| `IGitCommandRunner.RunCommands(...)` | `RunCommandsAsync(...)` (all overloads) |
| `IGitCommandRunner.RunCommandsOrNull(...)` | `RunCommandsOrNullAsync(...)` (all overloads) |

- The groups are `Repository`, `WorkingTree`, `Branches`, `Remotes`, `History`, and `Maintenance`.
  Look there for any command not listed above.
- `Tools.Git.Information.CommitsAsync(...)` returns the same commits as
  `Commands.History.CommitsAsync(...)`.
- Update custom `IGitCommandRunner` implementations and test doubles to the new method names.
- Keep forwarding cancellation tokens through the token-aware overloads, so running Git commands
  can be canceled. `RunCommandsOrNullAsync` still propagates cancellation rather than returning `null`.

If you are upgrading an intermediate V4 build that exposed `IGitInformation.Commits(...)`, rename
it to `IGitInformation.CommitsAsync(...)`. In released V3.2.8, `Commits` existed only on the
internal `GitInformation` implementation, not on its public interface.

Git branch run conditions are covered in
[Conditions and requirements](./conditions-and-requirements.md#git-branch-conditions).

## GitHub

- Rename explicit references to `SkipIfNoGitHubToken` to `SkipIfNoGitHubTokenAttribute`. The
  attribute syntax `[SkipIfNoGitHubToken]` is still valid.
- The generated `gh` CLI options and services keep their generated namespaces.
- `EnvironmentVariables.RunNumber` is now `int?`. See [Build agent variables](#build-agent-variables).

### Workflow file writer

- `GitHubPipelineFileWriter` is internal.
- `GitHubPipelineFileWriterOptions.RunnerOperatingSystem` is removed. Use `Runner`.
- `GitHubPipelineFileWriterOptions.ValuesToMask` is replaced by `SecretNamesToMask`. Pass GitHub
  secret **names** such as `"NUGET_API_KEY"`, not secret values or `${{ secrets.NUGET_API_KEY }}`
  expressions. The writer generates the secret expressions in the masking step's environment, so
  values are resolved only when GitHub Actions runs the workflow. Invalid secret names are
  rejected before the output file is written.

:::warning
If an earlier generated workflow contains literal secrets, revoke or rotate those secrets and
remove them from the repository's history.
:::

## Node.js, npm, and npx

The handwritten `ModularPipelines.Node.Models` types are removed. Use the generated options in
`ModularPipelines.Node.Options` and the service interfaces in `ModularPipelines.Node.Services`.
These do not move to the package root namespace. See
[npm and npx in V4](../mp-packages/node.md#npm-and-npx-in-v4).

## TRX test results

Rename `ITrx.ParseTrxFile(file)` to `ParseTrxFileAsync(file, cancellationToken)`. The token is
optional and cancels the asynchronous file read.

```csharp
var results = await context.Tools.Trx.ParseTrxFileAsync(trxFile, cancellationToken);
```

## Azure

### Resource identifiers need a resource type

`AzureResourceIdentifier` now requires a resource type before the resource name. The scope
factories change the same way.

```csharp
var scope = new AzureResourceIdentifier(
    subscriptionId, resourceGroupName, "Microsoft.Web/sites", siteName);

subscription.GetResourceIdentifier(resourceGroupName, "Microsoft.Web/sites", siteName);
resourceGroup.GetResourceIdentifier("Microsoft.Web/sites", siteName);
```

- Converting an Azure SDK `ResourceIdentifier` now keeps its resource type.
- Resource lookup matches both type and name with `OrdinalIgnoreCase`, so resources of different
  types can safely share a name.
- A missing resource still throws `AzureResourceNotFoundException`.
- Subscription, resource-group, and management-group scopes resolve as before.
- `ArmClient.GetResourceIdentifierAsync` accepts an optional final `CancellationToken`. Pass one to
  cancel resource enumeration.

### Provisioner methods end in `Async`

The methods under `context.Tools.Azure.Provisioner` gain the `Async` suffix. For example,
`AuthorizationRoleDefinition` becomes `AuthorizationRoleDefinitionAsync`. That method also forwards
its token when it resolves the scope.

## AWS Lambda SDK wrapper

The handwritten `ModularPipelines.AmazonWebServices.Lambda` class is removed. It only forwarded
calls to `AmazonLambdaClient`, and was never registered or exposed through the integration context.

1. Add an explicit `AWSSDK.Lambda` package reference if your pipeline uses the SDK. The integration
   package no longer references it transitively.
2. Call the matching `AmazonLambdaClient` method, with the `Async` suffix:

```csharp
// V3
// var lambda = new ModularPipelines.AmazonWebServices.Lambda(client);
// await lambda.Invoke(request, cancellationToken);

// V4
await client.InvokeAsync(request, cancellationToken);
```

Your application owns the client, including its configuration, registration, and disposal.

For CLI operations, use the separate generated `context.Tools.Aws.Lambda` service. Its methods run
`aws lambda` commands and return command results, not SDK responses.

## Build agent variables

Build-agent variable accessors are nullable where the variable can be absent. Add null handling
where you read them.

| Package | Change |
| --- | --- |
| `ModularPipelines.GitHub` | `EnvironmentVariables.RunNumber` is `int?` |
| `ModularPipelines.Azure.Pipelines` | The agent variables are `string?` |
