using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("boards", "work-item", "show")]
public record AzBoardsWorkItemShowOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliOption("--as-of")]
    public string? AsOf { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--expand")]
    public string? Expand { get; set; }

    [CliOption("--fields")]
    public string? Fields { get; set; }

    [CliFlag("--open")]
    public bool? Open { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }
}