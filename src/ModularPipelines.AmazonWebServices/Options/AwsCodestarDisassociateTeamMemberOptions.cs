using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar", "disassociate-team-member")]
public record AwsCodestarDisassociateTeamMemberOptions(
[property: CliOption("--project-id")] string ProjectId,
[property: CliOption("--user-arn")] string UserArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}