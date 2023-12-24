using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "create-resource")]
public record AwsApigatewayCreateResourceOptions(
[property: CommandSwitch("--rest-api-id")] string RestApiId,
[property: CommandSwitch("--parent-id")] string ParentId,
[property: CommandSwitch("--path-part")] string PathPart
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}