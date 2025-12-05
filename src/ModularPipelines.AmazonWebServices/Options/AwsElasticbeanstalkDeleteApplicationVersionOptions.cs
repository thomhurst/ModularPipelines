using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "delete-application-version")]
public record AwsElasticbeanstalkDeleteApplicationVersionOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--version-label")] string VersionLabel
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}