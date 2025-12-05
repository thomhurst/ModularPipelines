using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "create-platform-version")]
public record AwsElasticbeanstalkCreatePlatformVersionOptions(
[property: CliOption("--platform-name")] string PlatformName,
[property: CliOption("--platform-version")] string PlatformVersion,
[property: CliOption("--platform-definition-bundle")] string PlatformDefinitionBundle
) : AwsOptions
{
    [CliOption("--environment-name")]
    public string? EnvironmentName { get; set; }

    [CliOption("--option-settings")]
    public string[]? OptionSettings { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}