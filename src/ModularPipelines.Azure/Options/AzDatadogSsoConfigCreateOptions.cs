using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datadog", "sso-config", "create")]
public record AzDatadogSsoConfigCreateOptions(
[property: CliOption("--configuration-name")] string ConfigurationName,
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--properties")]
    public string? Properties { get; set; }
}