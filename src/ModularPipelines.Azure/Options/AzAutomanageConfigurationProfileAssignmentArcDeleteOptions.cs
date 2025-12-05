using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("automanage", "configuration-profile-assignment", "arc", "delete")]
public record AzAutomanageConfigurationProfileAssignmentArcDeleteOptions : AzOptions
{
    [CliOption("--configuration-profile-assignment-name")]
    public string? ConfigurationProfileAssignmentName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--machine-name")]
    public string? MachineName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}