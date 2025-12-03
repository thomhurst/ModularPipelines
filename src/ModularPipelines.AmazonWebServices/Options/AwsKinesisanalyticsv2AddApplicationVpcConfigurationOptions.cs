using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("kinesisanalyticsv2", "add-application-vpc-configuration")]
public record AwsKinesisanalyticsv2AddApplicationVpcConfigurationOptions(
[property: CliOption("--application-name")] string ApplicationName,
[property: CliOption("--vpc-configuration")] string VpcConfiguration
) : AwsOptions
{
    [CliOption("--current-application-version-id")]
    public long? CurrentApplicationVersionId { get; set; }

    [CliOption("--conditional-token")]
    public string? ConditionalToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}