using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "create-configuration-template")]
public record AwsElasticbeanstalkCreateConfigurationTemplateOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--template-name")] string TemplateName
) : AwsOptions
{
    [CliOption("--solution-stack-name")]
    public string? SolutionStackName { get; set; }

    [CliOption("--platform-arn")]
    public string? PlatformArn { get; set; }

    [CliOption("--source-configuration")]
    public string? SourceConfiguration { get; set; }

    [CliOption("--environment-id")]
    public string? EnvironmentId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--option-settings")]
    public string[]? OptionSettings { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}