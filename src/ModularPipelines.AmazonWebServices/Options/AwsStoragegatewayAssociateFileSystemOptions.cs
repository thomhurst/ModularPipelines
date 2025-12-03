using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("storagegateway", "associate-file-system")]
public record AwsStoragegatewayAssociateFileSystemOptions(
[property: CliOption("--user-name")] string UserName,
[property: CliOption("--password")] string Password,
[property: CliOption("--client-token")] string ClientToken,
[property: CliOption("--gateway-arn")] string GatewayArn,
[property: CliOption("--location-arn")] string LocationArn
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--audit-destination-arn")]
    public string? AuditDestinationArn { get; set; }

    [CliOption("--cache-attributes")]
    public string? CacheAttributes { get; set; }

    [CliOption("--endpoint-network-configuration")]
    public string? EndpointNetworkConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}