using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalyticsv2", "delete-application-vpc-configuration")]
public record AwsKinesisanalyticsv2DeleteApplicationVpcConfigurationOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--vpc-configuration-id")] string VpcConfigurationId
) : AwsOptions
{
    [CliOption("--current-application-version-id")]
    public long? CurrentApplicationVersionId { get; set; }

    [CliOption("--conditional-token")]
    public string? ConditionalToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}