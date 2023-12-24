using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "delete-model")]
public record AwsApigatewayv2DeleteModelOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--model-id")] string ModelId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}