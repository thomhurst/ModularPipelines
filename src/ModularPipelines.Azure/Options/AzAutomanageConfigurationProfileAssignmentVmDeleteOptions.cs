using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("automanage", "configuration-profile-assignment", "vm", "delete")]
public record AzAutomanageConfigurationProfileAssignmentVmDeleteOptions : AzOptions
{
    [CommandSwitch("--configuration-profile-assignment-name")]
    public string? ConfigurationProfileAssignmentName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--vm-name")]
    public string? VmName { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}