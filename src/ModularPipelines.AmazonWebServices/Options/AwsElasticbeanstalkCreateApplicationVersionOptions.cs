using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "create-application-version")]
public record AwsElasticbeanstalkCreateApplicationVersionOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--version-label")] string VersionLabel
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--source-build-information")]
    public string? SourceBuildInformation { get; set; }

    [CliOption("--source-bundle")]
    public string? SourceBundle { get; set; }

    [CliOption("--build-configuration")]
    public string? BuildConfiguration { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}