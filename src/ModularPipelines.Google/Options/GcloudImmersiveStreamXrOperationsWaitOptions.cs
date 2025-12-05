using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("immersive-stream", "xr", "operations", "wait")]
public record GcloudImmersiveStreamXrOperationsWaitOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Location
) : GcloudOptions;