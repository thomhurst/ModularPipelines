using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-gateway", "get-gateway")]
public record AwsBackupGatewayGetGatewayOptions(
[property: CliOption("--gateway-arn")] string GatewayArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}