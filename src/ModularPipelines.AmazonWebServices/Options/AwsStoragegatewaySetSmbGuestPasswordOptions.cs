using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "set-smb-guest-password")]
public record AwsStoragegatewaySetSmbGuestPasswordOptions(
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--password")] string Password
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}