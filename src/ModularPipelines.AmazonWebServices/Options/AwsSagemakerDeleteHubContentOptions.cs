using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "delete-hub-content")]
public record AwsSagemakerDeleteHubContentOptions(
[property: CliOption("--hub-name")] string HubName,
[property: CliOption("--hub-content-type")] string HubContentType,
[property: CliOption("--hub-content-name")] string HubContentName,
[property: CliOption("--hub-content-version")] string HubContentVersion
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}