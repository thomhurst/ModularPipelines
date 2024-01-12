using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("assured", "workloads", "violations", "acknowledge")]
public record GcloudAssuredWorkloadsViolationsAcknowledgeOptions(
[property: PositionalArgument] string Violation,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Organization,
[property: PositionalArgument] string Workload,
[property: CommandSwitch("--comment")] string Comment
) : GcloudOptions
{
    [CommandSwitch("--acknowledge-type")]
    public string? AcknowledgeType { get; set; }
}