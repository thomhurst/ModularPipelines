using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "describe-endpoint-access")]
public record AwsRedshiftDescribeEndpointAccessOptions : AwsOptions
{
    [CommandSwitch("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CommandSwitch("--resource-owner")]
    public string? ResourceOwner { get; set; }

    [CommandSwitch("--endpoint-name")]
    public string? EndpointName { get; set; }

    [CommandSwitch("--vpc-id")]
    public string? VpcId { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}