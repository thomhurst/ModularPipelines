using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sentinel", "source-control", "list-repository")]
public record AzSentinelSourceControlListRepositoryOptions(
[property: CommandSwitch("--repo-type")] string RepoType,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--workspace-name")] string WorkspaceName
) : AzOptions;