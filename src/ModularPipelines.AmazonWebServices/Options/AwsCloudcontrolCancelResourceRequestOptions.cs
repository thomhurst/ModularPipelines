using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudcontrol", "cancel-resource-request")]
public record AwsCloudcontrolCancelResourceRequestOptions(
[property: CliOption("--request-token")] string RequestToken
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}