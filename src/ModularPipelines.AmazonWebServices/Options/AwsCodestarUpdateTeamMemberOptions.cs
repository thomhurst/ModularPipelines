using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codestar", "update-team-member")]
public record AwsCodestarUpdateTeamMemberOptions(
[property: CliOption("--project-id")] string ProjectId,
[property: CliOption("--user-arn")] string UserArn
) : AwsOptions
{
    [CliOption("--project-role")]
    public string? ProjectRole { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}