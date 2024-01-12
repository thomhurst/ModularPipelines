using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "repositories", "list-cleanup-policies")]
public record GcloudArtifactsRepositoriesListCleanupPoliciesOptions(
[property: PositionalArgument] string Repository,
[property: PositionalArgument] string Location
) : GcloudOptions;