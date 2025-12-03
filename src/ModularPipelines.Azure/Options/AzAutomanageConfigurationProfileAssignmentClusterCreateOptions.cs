using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("automanage", "configuration-profile-assignment", "cluster", "create")]
public record AzAutomanageConfigurationProfileAssignmentClusterCreateOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--configuration-profile-assignment-name")] string ConfigurationProfileAssignmentName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--configuration-profile")]
    public string? ConfigurationProfile { get; set; }
}