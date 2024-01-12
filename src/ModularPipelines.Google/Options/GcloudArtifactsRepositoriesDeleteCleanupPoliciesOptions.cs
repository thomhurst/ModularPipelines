using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "repositories", "delete-cleanup-policies")]
public record GcloudArtifactsRepositoriesDeleteCleanupPoliciesOptions(
[property: PositionalArgument] string Repository,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--policynames")] string Policynames
) : GcloudOptions;