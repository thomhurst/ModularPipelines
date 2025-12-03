using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "patch-jobs", "describe")]
public record GcloudComputeOsConfigPatchJobsDescribeOptions(
[property: CliArgument] string PatchJob
) : GcloudOptions;