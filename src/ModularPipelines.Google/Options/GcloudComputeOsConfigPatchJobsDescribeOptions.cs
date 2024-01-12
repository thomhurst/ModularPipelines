using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-config", "patch-jobs", "describe")]
public record GcloudComputeOsConfigPatchJobsDescribeOptions(
[property: PositionalArgument] string PatchJob
) : GcloudOptions;