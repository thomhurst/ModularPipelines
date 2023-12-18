using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage", "configuration-profile-assignment", "arc", "create")]
public record AzAutomanageConfigurationProfileAssignmentArcCreateOptions(
[property: CommandSwitch("--configuration-profile-assignment-name")] string ConfigurationProfileAssignmentName,
[property: CommandSwitch("--machine-name")] string MachineName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--configuration-profile")]
    public string? ConfigurationProfile { get; set; }
}