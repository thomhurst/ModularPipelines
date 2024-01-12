using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-config", "patch-jobs", "list-instance-details")]
public record GcloudComputeOsConfigPatchJobsListInstanceDetailsOptions(
[property: PositionalArgument] string PatchJob
) : GcloudOptions;