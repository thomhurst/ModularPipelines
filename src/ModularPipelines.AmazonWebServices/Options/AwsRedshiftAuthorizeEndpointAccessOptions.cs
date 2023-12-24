using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "authorize-endpoint-access")]
public record AwsRedshiftAuthorizeEndpointAccessOptions(
[property: CommandSwitch("--account")] string Account
) : AwsOptions
{
    [CommandSwitch("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CommandSwitch("--vpc-ids")]
    public string[]? VpcIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}