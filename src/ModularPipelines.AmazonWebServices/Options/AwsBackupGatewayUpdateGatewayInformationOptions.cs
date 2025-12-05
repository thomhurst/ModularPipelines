using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-gateway", "update-gateway-information")]
public record AwsBackupGatewayUpdateGatewayInformationOptions(
[property: CliOption("--gateway-arn")] string GatewayArn
) : AwsOptions
{
    [CliOption("--gateway-display-name")]
    public string? GatewayDisplayName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}