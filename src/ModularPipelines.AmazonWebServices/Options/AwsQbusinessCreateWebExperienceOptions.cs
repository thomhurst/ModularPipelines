using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("qbusiness", "create-web-experience")]
public record AwsQbusinessCreateWebExperienceOptions(
[property: CliOption("--application-id")] string ApplicationId
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--sample-prompts-control-mode")]
    public string? SamplePromptsControlMode { get; set; }

    [CliOption("--subtitle")]
    public string? Subtitle { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--title")]
    public string? Title { get; set; }

    [CliOption("--welcome-message")]
    public string? WelcomeMessage { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}