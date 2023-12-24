using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "get-vpc-link")]
public record AwsApigatewayGetVpcLinkOptions(
[property: CommandSwitch("--vpc-link-id")] string VpcLinkId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}