using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("services", "enable")]
public record GcloudServicesEnableOptions(
[property: CliArgument] string Service
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}