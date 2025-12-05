using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("repos", "policy", "file-size", "update")]
public record AzReposPolicyFileSizeUpdateOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliFlag("--blocking")]
    public bool? Blocking { get; set; }

    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliOption("--maximum-git-blob-size")]
    public string? MaximumGitBlobSize { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--repository-id")]
    public string? RepositoryId { get; set; }

    [CliFlag("--use-uncompressed-size")]
    public bool? UseUncompressedSize { get; set; }
}