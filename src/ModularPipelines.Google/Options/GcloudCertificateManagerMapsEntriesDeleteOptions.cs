using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("certificate-manager", "maps", "entries", "delete")]
public record GcloudCertificateManagerMapsEntriesDeleteOptions(
[property: CliArgument] string Entry,
[property: CliArgument] string Location,
[property: CliArgument] string Map
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}