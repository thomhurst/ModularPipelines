using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "interconnects", "delete")]
public record GcloudComputeInterconnectsDeleteOptions(
[property: CliArgument] string Name
) : GcloudOptions;