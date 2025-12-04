using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("pipelines", "list")]
public record AzPipelinesListOptions : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--folder-path")]
    public string? FolderPath { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--query-order")]
    public string? QueryOrder { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }

    [CliOption("--repository-type")]
    public string? RepositoryType { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }
}