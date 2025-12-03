using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("domains", "registrations", "delete")]
public record GcloudDomainsRegistrationsDeleteOptions(
[property: CliArgument] string Registration
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}