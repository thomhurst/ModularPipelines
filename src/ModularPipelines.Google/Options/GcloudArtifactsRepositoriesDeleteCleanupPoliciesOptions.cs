using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "repositories", "delete-cleanup-policies")]
public record GcloudArtifactsRepositoriesDeleteCleanupPoliciesOptions(
[property: CliArgument] string Repository,
[property: CliArgument] string Location,
[property: CliOption("--policynames")] string Policynames
) : GcloudOptions;