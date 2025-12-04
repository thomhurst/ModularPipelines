using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "source-control", "create")]
public record AzSentinelSourceControlCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--content-types")]
    public string? ContentTypes { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--etag")]
    public string? Etag { get; set; }

    [CliOption("--id")]
    public string? Id { get; set; }

    [CliOption("--last-deployment-info")]
    public string? LastDeploymentInfo { get; set; }

    [CliOption("--repo-type")]
    public string? RepoType { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }

    [CliOption("--repository-info")]
    public string? RepositoryInfo { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}