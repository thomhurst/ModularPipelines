using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "os-config", "patch-jobs", "cancel")]
public record GcloudComputeOsConfigPatchJobsCancelOptions(
[property: CliArgument] string PatchJob
) : GcloudOptions;