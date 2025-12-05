using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "delete-git-hub-account-token")]
public record AwsDeployDeleteGitHubAccountTokenOptions : AwsOptions
{
    [CliOption("--token-name")]
    public string? TokenName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}