using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("repos", "policy", "file-size", "create")]
public record AzReposPolicyFileSizeCreateOptions(
[property: CliFlag("--blocking")] bool Blocking,
[property: CliFlag("--enabled")] bool Enabled,
[property: CliOption("--maximum-git-blob-size")] string MaximumGitBlobSize,
[property: CliOption("--repository-id")] string RepositoryId,
[property: CliFlag("--use-uncompressed-size")] bool UseUncompressedSize
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}