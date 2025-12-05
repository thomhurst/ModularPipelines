using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "mi", "server-configuration-option", "set")]
public record AzSqlMiServerConfigurationOptionSetOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--instance-name")]
    public string? InstanceName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--server-configuration-option-value")]
    public string? ServerConfigurationOptionValue { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}