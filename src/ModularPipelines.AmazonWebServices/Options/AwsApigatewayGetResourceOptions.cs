using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "get-resource")]
public record AwsApigatewayGetResourceOptions(
[property: CommandSwitch("--rest-api-id")] string RestApiId,
[property: CommandSwitch("--resource-id")] string ResourceId
) : AwsOptions
{
    [CommandSwitch("--embed")]
    public string[]? Embed { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}