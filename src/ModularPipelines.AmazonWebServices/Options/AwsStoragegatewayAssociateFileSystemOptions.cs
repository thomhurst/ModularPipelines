using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "associate-file-system")]
public record AwsStoragegatewayAssociateFileSystemOptions(
[property: CommandSwitch("--user-name")] string UserName,
[property: CommandSwitch("--password")] string Password,
[property: CommandSwitch("--client-token")] string ClientToken,
[property: CommandSwitch("--gateway-arn")] string GatewayArn,
[property: CommandSwitch("--location-arn")] string LocationArn
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--audit-destination-arn")]
    public string? AuditDestinationArn { get; set; }

    [CommandSwitch("--cache-attributes")]
    public string? CacheAttributes { get; set; }

    [CommandSwitch("--endpoint-network-configuration")]
    public string? EndpointNetworkConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}