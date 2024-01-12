using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "instances", "describe")]
public record GcloudSqlInstancesDescribeOptions(
[property: PositionalArgument] string Instance
) : GcloudOptions;