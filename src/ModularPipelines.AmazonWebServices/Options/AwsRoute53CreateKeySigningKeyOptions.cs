using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("route53", "create-key-signing-key")]
public record AwsRoute53CreateKeySigningKeyOptions(
[property: CliOption("--caller-reference")] string CallerReference,
[property: CliOption("--hosted-zone-id")] string HostedZoneId,
[property: CliOption("--key-management-service-arn")] string KeyManagementServiceArn,
[property: CliOption("--name")] string Name,
[property: CliOption("--status")] string Status
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}