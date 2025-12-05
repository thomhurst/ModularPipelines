using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticbeanstalk", "update-application-resource-lifecycle")]
public record AwsElasticbeanstalkUpdateApplicationResourceLifecycleOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--resource-lifecycle-config")] string ResourceLifecycleConfig
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}