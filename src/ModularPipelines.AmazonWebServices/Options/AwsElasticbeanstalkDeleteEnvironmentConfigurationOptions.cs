using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "delete-environment-configuration")]
public record AwsElasticbeanstalkDeleteEnvironmentConfigurationOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--environment-name")] string EnvironmentName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}