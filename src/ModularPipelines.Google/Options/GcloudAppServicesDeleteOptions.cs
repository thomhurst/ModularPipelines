using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("app", "services", "delete")]
public record GcloudAppServicesDeleteOptions(
[property: CliArgument] string Services
) : GcloudOptions
{
    [CliOption("--version")]
    public new string? Version { get; set; }
}