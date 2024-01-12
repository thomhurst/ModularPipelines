using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataflow", "jobs", "list")]
public record GcloudDataflowJobsListOptions : GcloudOptions
{
    [CommandSwitch("--created-after")]
    public string? CreatedAfter { get; set; }

    [CommandSwitch("--created-before")]
    public string? CreatedBefore { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--status")]
    public string? Status { get; set; }
}