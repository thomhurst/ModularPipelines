using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "create")]
public record AzDtCreateOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--mi-system-assigned")]
    public bool? MiSystemAssigned { get; set; }

    [CommandSwitch("--mi-user-assigned")]
    public string? MiUserAssigned { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--pna")]
    public string? Pna { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--scopes")]
    public string? Scopes { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}