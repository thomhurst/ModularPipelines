using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "instances", "delete")]
public record GcloudSqlInstancesDeleteOptions(
[property: CliArgument] string Instance
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}