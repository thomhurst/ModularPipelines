using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "instances", "describe")]
public record GcloudSqlInstancesDescribeOptions(
[property: CliArgument] string Instance
) : GcloudOptions;