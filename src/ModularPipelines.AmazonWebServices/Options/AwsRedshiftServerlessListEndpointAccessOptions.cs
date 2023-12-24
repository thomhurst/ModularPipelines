using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "list-endpoint-access")]
public record AwsRedshiftServerlessListEndpointAccessOptions : AwsOptions
{
    [CommandSwitch("--owner-account")]
    public string? OwnerAccount { get; set; }

    [CommandSwitch("--vpc-id")]
    public string? VpcId { get; set; }

    [CommandSwitch("--workgroup-name")]
    public string? WorkgroupName { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}