using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("pipelines", "create")]
public record AzPipelinesCreateOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--branch")]
    public string? Branch { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--folder-path")]
    public string? FolderPath { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--queue-id")]
    public string? QueueId { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }

    [CliOption("--repository-type")]
    public string? RepositoryType { get; set; }

    [CliOption("--service-connection")]
    public string? ServiceConnection { get; set; }

    [CliFlag("--skip-first-run")]
    public bool? SkipFirstRun { get; set; }

    [CliOption("--yaml-path")]
    public string? YamlPath { get; set; }
}