using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "put-rest-api")]
public record AwsApigatewayPutRestApiOptions(
[property: CommandSwitch("--rest-api-id")] string RestApiId,
[property: CommandSwitch("--body")] string Body
) : AwsOptions
{
    [CommandSwitch("--mode")]
    public string? Mode { get; set; }

    [CommandSwitch("--parameters")]
    public IEnumerable<KeyValue>? Parameters { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}