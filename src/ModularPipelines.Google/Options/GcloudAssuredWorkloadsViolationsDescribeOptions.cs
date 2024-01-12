using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("assured", "workloads", "violations", "describe")]
public record GcloudAssuredWorkloadsViolationsDescribeOptions(
[property: PositionalArgument] string Violation,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Organization,
[property: PositionalArgument] string Workload
) : GcloudOptions;