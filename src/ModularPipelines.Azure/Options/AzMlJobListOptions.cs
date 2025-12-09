using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "job", "list")]
public record AzMlJobListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliFlag("--all-results")]
    public bool? AllResults { get; set; }

    [CliFlag("--archived-only")]
    public bool? ArchivedOnly { get; set; }

    [CliFlag("--include-archived")]
    public bool? IncludeArchived { get; set; }

    [CliOption("--max-results")]
    public string? MaxResults { get; set; }

    [CliOption("--parent-job-name")]
    public string? ParentJobName { get; set; }
}