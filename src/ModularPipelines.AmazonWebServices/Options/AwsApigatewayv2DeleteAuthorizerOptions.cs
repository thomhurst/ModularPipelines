using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "delete-authorizer")]
public record AwsApigatewayv2DeleteAuthorizerOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--authorizer-id")] string AuthorizerId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}