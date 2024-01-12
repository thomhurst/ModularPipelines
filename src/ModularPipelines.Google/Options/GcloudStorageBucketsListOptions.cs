using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "buckets", "list")]
public record GcloudStorageBucketsListOptions(
[property: PositionalArgument] string Urls
) : GcloudOptions
{
    [CommandSwitch("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [BooleanCommandSwitch("--raw")]
    public bool? Raw { get; set; }
}