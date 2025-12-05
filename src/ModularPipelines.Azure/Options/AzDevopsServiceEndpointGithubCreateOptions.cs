using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devops", "service-endpoint", "github", "create")]
public record AzDevopsServiceEndpointGithubCreateOptions(
[property: CliOption("--github-url")] string GithubUrl,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--detect")]
    public bool? Detect { get; set; }

    [CliOption("--org")]
    public string? Org { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }
}