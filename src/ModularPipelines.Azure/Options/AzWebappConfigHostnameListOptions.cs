using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("webapp", "config", "hostname", "list")]
public record AzWebappConfigHostnameListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--webapp-name")] string WebappName
) : AzOptions
{
    [CliOption("--slot")]
    public string? Slot { get; set; }
}