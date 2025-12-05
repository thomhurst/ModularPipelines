using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("pipelines", "update")]
public record AzPipelinesUpdateOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--new-folder-path")]
    public string? NewFolderPath { get; set; }

    [CliOption("--new-name")]
    public string? NewName { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--queue-id")]
    public string? QueueId { get; set; }

    [CliOption("--yaml-path")]
    public string? YamlPath { get; set; }
}