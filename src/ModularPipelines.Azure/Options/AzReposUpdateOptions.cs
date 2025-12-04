using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("repos", "update")]
public record AzReposUpdateOptions(
[property: CliOption("--repository")] string Repository
) : AzOptions
{
    [CliOption("--default-branch")]
    public string? DefaultBranch { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}