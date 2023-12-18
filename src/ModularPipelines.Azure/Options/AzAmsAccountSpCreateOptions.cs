using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "account", "sp", "create")]
public record AzAmsAccountSpCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--new-sp-name")]
    public string? NewSpName { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [BooleanCommandSwitch("--xml")]
    public bool? Xml { get; set; }

    [CommandSwitch("--years")]
    public string? Years { get; set; }
}