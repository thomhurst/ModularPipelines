using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group", "delete")]
public record AzAccountManagementGroupDeleteOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [BooleanCommandSwitch("--no-register")]
    public bool? NoRegister { get; set; }
}