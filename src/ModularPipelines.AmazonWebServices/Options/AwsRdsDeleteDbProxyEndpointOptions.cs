using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "delete-db-proxy-endpoint")]
public record AwsRdsDeleteDbProxyEndpointOptions(
[property: CommandSwitch("--db-proxy-endpoint-name")] string DbProxyEndpointName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}