using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "create-api-mapping")]
public record AwsApigatewayv2CreateApiMappingOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--stage")] string Stage
) : AwsOptions
{
    [CommandSwitch("--api-mapping-key")]
    public string? ApiMappingKey { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}