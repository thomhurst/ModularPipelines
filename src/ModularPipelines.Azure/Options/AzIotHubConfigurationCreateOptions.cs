using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "hub", "configuration", "create")]
public record AzIotHubConfigurationCreateOptions(
[property: CliOption("--config-id")] string ConfigId,
[property: CliOption("--content")] string Content
) : AzOptions
{
    [CliOption("--auth-type")]
    public string? AuthType { get; set; }

    [CliOption("--cl")]
    public string? Cl { get; set; }

    [CliOption("--cmq")]
    public string? Cmq { get; set; }

    [CliOption("--hub-name")]
    public string? HubName { get; set; }

    [CliOption("--lab")]
    public string? Lab { get; set; }

    [CliOption("--login")]
    public string? Login { get; set; }

    [CliOption("--metrics")]
    public string? Metrics { get; set; }

    [CliOption("--pri")]
    public string? Pri { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--target-condition")]
    public string? TargetCondition { get; set; }
}