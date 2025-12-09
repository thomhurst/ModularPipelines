using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("maintenance", "assignment", "show-subscription")]
public record AzMaintenanceAssignmentShowSubscriptionOptions : AzOptions
{
    [CliOption("--configuration-assignment-name")]
    public string? ConfigurationAssignmentName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}