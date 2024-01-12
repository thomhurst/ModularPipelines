using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "instances", "list")]
public record GcloudSqlInstancesListOptions : GcloudOptions
{
    [BooleanCommandSwitch("--show-edition")]
    public bool? ShowEdition { get; set; }

    [BooleanCommandSwitch("--show-sql-network-architecture")]
    public bool? ShowSqlNetworkArchitecture { get; set; }
}