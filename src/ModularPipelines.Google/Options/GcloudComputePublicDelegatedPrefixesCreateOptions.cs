using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "public-delegated-prefixes", "create")]
public record GcloudComputePublicDelegatedPrefixesCreateOptions(
[property: CliArgument] string Name,
[property: CliOption("--range")] string Range,
[property: CliOption("--public-advertised-prefix")] string PublicAdvertisedPrefix,
[property: CliOption("--public-delegated-prefix")] string PublicDelegatedPrefix
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--enable-live-migration")]
    public bool? EnableLiveMigration { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}