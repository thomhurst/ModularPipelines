using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group", "show")]
public record AzAccountManagementGroupShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--expand")]
    public bool? Expand { get; set; }

    [BooleanCommandSwitch("--no-register")]
    public bool? NoRegister { get; set; }

    [BooleanCommandSwitch("--recurse")]
    public bool? Recurse { get; set; }
}