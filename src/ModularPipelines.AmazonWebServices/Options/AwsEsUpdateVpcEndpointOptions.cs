using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("es", "update-vpc-endpoint")]
public record AwsEsUpdateVpcEndpointOptions(
[property: CommandSwitch("--vpc-endpoint-id")] string VpcEndpointId,
[property: CommandSwitch("--vpc-options")] string VpcOptions
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}