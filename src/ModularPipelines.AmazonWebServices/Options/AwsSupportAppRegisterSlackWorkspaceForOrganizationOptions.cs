using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("support-app", "register-slack-workspace-for-organization")]
public record AwsSupportAppRegisterSlackWorkspaceForOrganizationOptions(
[property: CliOption("--team-id")] string TeamId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}