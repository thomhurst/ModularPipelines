using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "update-api-mapping")]
public record AwsApigatewayv2UpdateApiMappingOptions(
[property: CommandSwitch("--api-id")] string ApiId,
[property: CommandSwitch("--api-mapping-id")] string ApiMappingId,
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--api-mapping-key")]
    public string? ApiMappingKey { get; set; }

    [CommandSwitch("--stage")]
    public string? Stage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}