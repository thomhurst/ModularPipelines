using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage", "configuration-profile-assignment", "vm", "create")]
public record AzAutomanageConfigurationProfileAssignmentVmCreateOptions(
[property: CommandSwitch("--configuration-profile-assignment-name")] string ConfigurationProfileAssignmentName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-name")] string VmName
) : AzOptions
{
    [CommandSwitch("--configuration-profile")]
    public string? ConfigurationProfile { get; set; }
}