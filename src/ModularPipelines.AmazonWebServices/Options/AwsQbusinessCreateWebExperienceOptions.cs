using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("qbusiness", "create-web-experience")]
public record AwsQbusinessCreateWebExperienceOptions(
[property: CommandSwitch("--application-id")] string ApplicationId
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--sample-prompts-control-mode")]
    public string? SamplePromptsControlMode { get; set; }

    [CommandSwitch("--subtitle")]
    public string? Subtitle { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--title")]
    public string? Title { get; set; }

    [CommandSwitch("--welcome-message")]
    public string? WelcomeMessage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}