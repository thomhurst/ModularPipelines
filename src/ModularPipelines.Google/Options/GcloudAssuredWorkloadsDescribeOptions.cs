using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("assured", "workloads", "describe")]
public record GcloudAssuredWorkloadsDescribeOptions(
[property: PositionalArgument] string Workload,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Organization
) : GcloudOptions;