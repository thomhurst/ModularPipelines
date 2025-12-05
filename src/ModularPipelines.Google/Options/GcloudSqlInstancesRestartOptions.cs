using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "instances", "restart")]
public record GcloudSqlInstancesRestartOptions(
[property: CliArgument] string Instance
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}