using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mq", "describe-configuration-revision")]
public record AwsMqDescribeConfigurationRevisionOptions(
[property: CliOption("--configuration-id")] string ConfigurationId,
[property: CliOption("--configuration-revision")] string ConfigurationRevision
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}