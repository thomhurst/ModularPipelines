using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "vm", "add-to-group")]
public record AzSqlVmAddToGroupOptions(
[property: CommandSwitch("--sqlvm-group")] string SqlvmGroup
) : AzOptions
{
    [CommandSwitch("--bootstrap-acc-pwd")]
    public string? BootstrapAccPwd { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--operator-acc-pwd")]
    public string? OperatorAccPwd { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--service-acc-pwd")]
    public string? ServiceAccPwd { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}