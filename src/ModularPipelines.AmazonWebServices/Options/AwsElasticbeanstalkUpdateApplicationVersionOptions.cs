using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "update-application-version")]
public record AwsElasticbeanstalkUpdateApplicationVersionOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--version-label")] string VersionLabel
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}