using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "os-config", "patch-jobs", "cancel")]
public record GcloudComputeOsConfigPatchJobsCancelOptions(
[property: PositionalArgument] string PatchJob
) : GcloudOptions;