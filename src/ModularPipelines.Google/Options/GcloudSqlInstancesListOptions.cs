using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "instances", "list")]
public record GcloudSqlInstancesListOptions : GcloudOptions
{
    [CliFlag("--show-edition")]
    public bool? ShowEdition { get; set; }

    [CliFlag("--show-sql-network-architecture")]
    public bool? ShowSqlNetworkArchitecture { get; set; }
}