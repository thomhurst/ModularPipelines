using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "update-endpoint-access")]
public record AwsRedshiftServerlessUpdateEndpointAccessOptions(
[property: CommandSwitch("--endpoint-name")] string EndpointName
) : AwsOptions
{
    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}