using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logic", "integration-account", "map", "list")]
public record AzLogicIntegrationAccountMapListOptions(
[property: CliOption("--integration-account")] int IntegrationAccount,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}