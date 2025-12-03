using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logic", "integration-account", "map", "create")]
public record AzLogicIntegrationAccountMapCreateOptions(
[property: CliOption("--integration-account")] int IntegrationAccount,
[property: CliOption("--map-name")] string MapName,
[property: CliOption("--map-type")] string MapType,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--content-type")]
    public string? ContentType { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--map-content")]
    public string? MapContent { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}