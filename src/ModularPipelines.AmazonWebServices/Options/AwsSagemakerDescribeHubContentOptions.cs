using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "describe-hub-content")]
public record AwsSagemakerDescribeHubContentOptions(
[property: CliOption("--hub-name")] string HubName,
[property: CliOption("--hub-content-type")] string HubContentType,
[property: CliOption("--hub-content-name")] string HubContentName
) : AwsOptions
{
    [CliOption("--hub-content-version")]
    public string? HubContentVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}