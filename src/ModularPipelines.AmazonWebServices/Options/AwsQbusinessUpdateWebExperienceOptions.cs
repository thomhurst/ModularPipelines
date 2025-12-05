using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "update-web-experience")]
public record AwsQbusinessUpdateWebExperienceOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--web-experience-id")] string WebExperienceId
) : AwsOptions
{
    [CliOption("--authentication-configuration")]
    public string? AuthenticationConfiguration { get; set; }

    [CliOption("--sample-prompts-control-mode")]
    public string? SamplePromptsControlMode { get; set; }

    [CliOption("--subtitle")]
    public string? Subtitle { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--welcome-message")]
    public string? WelcomeMessage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}