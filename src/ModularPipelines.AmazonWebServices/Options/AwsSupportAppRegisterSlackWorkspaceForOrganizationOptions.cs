using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("support-app", "register-slack-workspace-for-organization")]
public record AwsSupportAppRegisterSlackWorkspaceForOrganizationOptions(
[property: CommandSwitch("--team-id")] string TeamId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}