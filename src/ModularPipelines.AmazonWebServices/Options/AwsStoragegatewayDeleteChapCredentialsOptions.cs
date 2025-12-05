using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "delete-chap-credentials")]
public record AwsStoragegatewayDeleteChapCredentialsOptions(
[property: CliOption("--target-arn")] string TargetArn,
[property: CliOption("--initiator-name")] string InitiatorName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}