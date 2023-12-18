using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group", "check-name-availability")]
public record AzAccountManagementGroupCheckNameAvailabilityOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--no-register")]
    public bool? NoRegister { get; set; }

    [CommandSwitch("--parent")]
    public string? Parent { get; set; }
}