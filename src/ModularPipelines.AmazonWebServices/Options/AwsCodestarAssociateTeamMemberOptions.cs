using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar", "associate-team-member")]
public record AwsCodestarAssociateTeamMemberOptions(
[property: CommandSwitch("--project-id")] string ProjectId,
[property: CommandSwitch("--user-arn")] string UserArn,
[property: CommandSwitch("--project-role")] string ProjectRole
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}