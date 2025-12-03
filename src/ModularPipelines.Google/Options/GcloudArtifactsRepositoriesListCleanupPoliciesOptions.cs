using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "repositories", "list-cleanup-policies")]
public record GcloudArtifactsRepositoriesListCleanupPoliciesOptions(
[property: CliArgument] string Repository,
[property: CliArgument] string Location
) : GcloudOptions;