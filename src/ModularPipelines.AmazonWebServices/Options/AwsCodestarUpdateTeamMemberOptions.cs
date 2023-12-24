using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codestar", "update-team-member")]
public record AwsCodestarUpdateTeamMemberOptions(
[property: CommandSwitch("--project-id")] string ProjectId,
[property: CommandSwitch("--user-arn")] string UserArn
) : AwsOptions
{
    [CommandSwitch("--project-role")]
    public string? ProjectRole { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}