using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("maintenance", "assignment", "delete")]
public record AzMaintenanceAssignmentDeleteOptions : AzOptions
{
    [CliOption("--configuration-assignment-name")]
    public string? ConfigurationAssignmentName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--provider-name")]
    public string? ProviderName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-name")]
    public string? ResourceName { get; set; }

    [CliOption("--resource-type")]
    public string? ResourceType { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}