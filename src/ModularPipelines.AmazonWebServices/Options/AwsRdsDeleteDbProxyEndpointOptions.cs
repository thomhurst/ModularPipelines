using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "delete-db-proxy-endpoint")]
public record AwsRdsDeleteDbProxyEndpointOptions(
[property: CliOption("--db-proxy-endpoint-name")] string DbProxyEndpointName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}