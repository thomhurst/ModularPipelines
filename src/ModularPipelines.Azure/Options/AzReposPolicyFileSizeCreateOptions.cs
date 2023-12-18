using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("repos", "policy", "file-size", "create")]
public record AzReposPolicyFileSizeCreateOptions(
[property: BooleanCommandSwitch("--blocking")] bool Blocking,
[property: BooleanCommandSwitch("--enabled")] bool Enabled,
[property: CommandSwitch("--maximum-git-blob-size")] string MaximumGitBlobSize,
[property: CommandSwitch("--repository-id")] string RepositoryId,
[property: BooleanCommandSwitch("--use-uncompressed-size")] bool UseUncompressedSize
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }
}