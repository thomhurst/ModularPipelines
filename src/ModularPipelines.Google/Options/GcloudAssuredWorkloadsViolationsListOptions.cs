using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("assured", "workloads", "violations", "list")]
public record GcloudAssuredWorkloadsViolationsListOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--organization")] string Organization,
[property: CliOption("--workload")] string Workload
) : GcloudOptions;