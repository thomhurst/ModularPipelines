using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("assured", "workloads", "violations", "acknowledge")]
public record GcloudAssuredWorkloadsViolationsAcknowledgeOptions(
[property: CliArgument] string Violation,
[property: CliArgument] string Location,
[property: CliArgument] string Organization,
[property: CliArgument] string Workload,
[property: CliOption("--comment")] string Comment
) : GcloudOptions
{
    [CliOption("--acknowledge-type")]
    public string? AcknowledgeType { get; set; }
}