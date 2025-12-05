using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "update-environment")]
public record AwsElasticbeanstalkUpdateEnvironmentOptions : AwsOptions
{
    [CliOption("--application-name")]
    public string? ApplicationName { get; set; }

    [CliOption("--environment-id")]
    public string? EnvironmentId { get; set; }

    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliOption("--group-name")]
    public string? GroupName { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tier")]
    public string? Tier { get; set; }

    [CliOption("--version-label")]
    public string? VersionLabel { get; set; }

    [CliOption("--template-name")]
    public string? TemplateName { get; set; }

    [CliOption("--solution-stack-name")]
    public string? SolutionStackName { get; set; }

    [CliOption("--platform-arn")]
    public string? PlatformArn { get; set; }

    [CliOption("--option-settings")]
    public string[]? OptionSettings { get; set; }

    [CliOption("--options-to-remove")]
    public string[]? OptionsToRemove { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}