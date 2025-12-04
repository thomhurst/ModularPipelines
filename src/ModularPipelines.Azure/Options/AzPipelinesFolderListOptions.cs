using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("pipelines", "folder", "list")]
public record AzPipelinesFolderListOptions : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--query-order")]
    public string? QueryOrder { get; set; }
}