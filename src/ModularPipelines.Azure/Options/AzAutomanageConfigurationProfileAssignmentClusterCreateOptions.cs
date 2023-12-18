using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage", "configuration-profile-assignment", "cluster", "create")]
public record AzAutomanageConfigurationProfileAssignmentClusterCreateOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--configuration-profile-assignment-name")] string ConfigurationProfileAssignmentName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--configuration-profile")]
    public string? ConfigurationProfile { get; set; }
}