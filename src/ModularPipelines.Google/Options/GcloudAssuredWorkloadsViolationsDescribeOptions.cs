using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("assured", "workloads", "violations", "describe")]
public record GcloudAssuredWorkloadsViolationsDescribeOptions(
[property: CliArgument] string Violation,
[property: CliArgument] string Location,
[property: CliArgument] string Organization,
[property: CliArgument] string Workload
) : GcloudOptions;