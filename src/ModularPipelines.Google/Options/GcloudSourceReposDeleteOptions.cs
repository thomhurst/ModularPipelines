using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("source", "repos", "delete")]
public record GcloudSourceReposDeleteOptions(
[property: CliArgument] string RepositoryName
) : GcloudOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }
}