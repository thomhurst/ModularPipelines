using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "public-delegated-prefixes", "create")]
public record GcloudComputePublicDelegatedPrefixesCreateOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--range")] string Range,
[property: CommandSwitch("--public-advertised-prefix")] string PublicAdvertisedPrefix,
[property: CommandSwitch("--public-delegated-prefix")] string PublicDelegatedPrefix
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--enable-live-migration")]
    public bool? EnableLiveMigration { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}