using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "update-web-experience")]
public record AwsQbusinessUpdateWebExperienceOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--web-experience-id")] string WebExperienceId
) : AwsOptions
{
    [CommandSwitch("--authentication-configuration")]
    public string? AuthenticationConfiguration { get; set; }

    [CommandSwitch("--sample-prompts-control-mode")]
    public string? SamplePromptsControlMode { get; set; }

    [CommandSwitch("--subtitle")]
    public string? Subtitle { get; set; }

    [CommandSwitch("--title")]
    public string? Title { get; set; }

    [CommandSwitch("--welcome-message")]
    public string? WelcomeMessage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}