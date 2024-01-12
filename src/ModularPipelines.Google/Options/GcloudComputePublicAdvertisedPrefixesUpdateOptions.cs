using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "public-advertised-prefixes", "update")]
public record GcloudComputePublicAdvertisedPrefixesUpdateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--status")] string Status
) : GcloudOptions;