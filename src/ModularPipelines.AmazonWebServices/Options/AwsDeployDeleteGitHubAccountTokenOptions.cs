using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "delete-git-hub-account-token")]
public record AwsDeployDeleteGitHubAccountTokenOptions : AwsOptions
{
    [CommandSwitch("--token-name")]
    public string? TokenName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}