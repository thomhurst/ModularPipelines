using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("artifacts", "repositories", "set-cleanup-policies")]
public record GcloudArtifactsRepositoriesSetCleanupPoliciesOptions(
[property: CliArgument] string Repository,
[property: CliArgument] string Location,
[property: CliFlag("--dry-run")] bool DryRun,
[property: CliOption("--policy")] string Policy
) : GcloudOptions;