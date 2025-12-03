using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storage", "cat")]
public record GcloudStorageCatOptions(
[property: CliArgument] string Url
) : GcloudOptions
{
    [CliOption("--additional-headers")]
    public string? AdditionalHeaders { get; set; }

    [CliFlag("--display-url")]
    public bool? DisplayUrl { get; set; }

    [CliOption("--range")]
    public string? Range { get; set; }
}