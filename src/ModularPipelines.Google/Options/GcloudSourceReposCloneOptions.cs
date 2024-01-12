using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("source", "repos", "clone")]
public record GcloudSourceReposCloneOptions(
[property: PositionalArgument] string RepositoryName,
[property: PositionalArgument] string DirectoryName
) : GcloudOptions
{
    [BooleanCommandSwitch("--dry-run")]
    public bool? DryRun { get; set; }
}