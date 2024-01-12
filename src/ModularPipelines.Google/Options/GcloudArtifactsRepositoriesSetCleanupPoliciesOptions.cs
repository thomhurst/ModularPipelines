using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("artifacts", "repositories", "set-cleanup-policies")]
public record GcloudArtifactsRepositoriesSetCleanupPoliciesOptions(
[property: PositionalArgument] string Repository,
[property: PositionalArgument] string Location,
[property: BooleanCommandSwitch("--dry-run")] bool DryRun,
[property: CommandSwitch("--policy")] string Policy
) : GcloudOptions;