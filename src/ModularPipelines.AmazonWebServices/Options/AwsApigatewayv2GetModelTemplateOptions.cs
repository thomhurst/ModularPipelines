using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "get-model-template")]
public record AwsApigatewayv2GetModelTemplateOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--model-id")] string ModelId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}