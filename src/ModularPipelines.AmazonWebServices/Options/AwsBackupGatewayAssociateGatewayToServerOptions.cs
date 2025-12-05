using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-gateway", "associate-gateway-to-server")]
public record AwsBackupGatewayAssociateGatewayToServerOptions(
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--server-arn")] string ServerArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}