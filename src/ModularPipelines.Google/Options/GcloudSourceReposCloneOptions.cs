using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("source", "repos", "clone")]
public record GcloudSourceReposCloneOptions(
[property: CliArgument] string RepositoryName,
[property: CliArgument] string DirectoryName
) : GcloudOptions
{
    [CliFlag("--dry-run")]
    public bool? DryRun { get; set; }
}