using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "vm", "add-to-group")]
public record AzSqlVmAddToGroupOptions(
[property: CliOption("--sqlvm-group")] string SqlvmGroup
) : AzOptions
{
    [CliOption("--bootstrap-acc-pwd")]
    public string? BootstrapAccPwd { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--operator-acc-pwd")]
    public string? OperatorAccPwd { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--service-acc-pwd")]
    public string? ServiceAccPwd { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}