using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("es", "delete-vpc-endpoint")]
public record AwsEsDeleteVpcEndpointOptions(
[property: CommandSwitch("--vpc-endpoint-id")] string VpcEndpointId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}