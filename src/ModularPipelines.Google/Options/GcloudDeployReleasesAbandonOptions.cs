using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "releases", "abandon")]
public record GcloudDeployReleasesAbandonOptions(
[property: PositionalArgument] string Release,
[property: PositionalArgument] string DeliveryPipeline,
[property: PositionalArgument] string Region
) : GcloudOptions;