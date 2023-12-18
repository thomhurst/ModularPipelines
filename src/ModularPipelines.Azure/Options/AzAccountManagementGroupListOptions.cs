using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("account", "management-group", "list")]
public record AzAccountManagementGroupListOptions : AzOptions
{
    [BooleanCommandSwitch("--no-register")]
    public bool? NoRegister { get; set; }
}