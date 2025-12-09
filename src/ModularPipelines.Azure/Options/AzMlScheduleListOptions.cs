using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "schedule", "list")]
public record AzMlScheduleListOptions(
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--disabled-only")]
    public bool? DisabledOnly { get; set; }

    [CliFlag("--include-disabled")]
    public bool? IncludeDisabled { get; set; }

    [CliOption("--max-results")]
    public string? MaxResults { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}