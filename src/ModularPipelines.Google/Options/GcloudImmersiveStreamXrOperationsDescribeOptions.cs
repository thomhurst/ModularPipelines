using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("immersive-stream", "xr", "operations", "describe")]
public record GcloudImmersiveStreamXrOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location
) : GcloudOptions;