using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("boards", "query")]
public record AzBoardsQueryOptions : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--wiql")]
    public string? Wiql { get; set; }
}