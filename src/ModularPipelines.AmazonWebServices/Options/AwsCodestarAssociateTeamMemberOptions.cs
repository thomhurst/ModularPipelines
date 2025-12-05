using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar", "associate-team-member")]
public record AwsCodestarAssociateTeamMemberOptions(
[property: CliOption("--project-id")] string ProjectId,
[property: CliOption("--user-arn")] string UserArn,
[property: CliOption("--project-role")] string ProjectRole
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}