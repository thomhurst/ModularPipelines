using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("certificate-manager", "maps", "entries", "delete")]
public record GcloudCertificateManagerMapsEntriesDeleteOptions(
[property: PositionalArgument] string Entry,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Map
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}