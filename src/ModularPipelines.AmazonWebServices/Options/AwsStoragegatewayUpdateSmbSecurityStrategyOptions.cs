using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "update-smb-security-strategy")]
public record AwsStoragegatewayUpdateSmbSecurityStrategyOptions(
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--smb-security-strategy")] string SmbSecurityStrategy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}