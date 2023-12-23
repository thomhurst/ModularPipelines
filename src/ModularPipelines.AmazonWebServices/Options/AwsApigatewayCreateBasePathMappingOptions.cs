using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "create-base-path-mapping")]
public record AwsApigatewayCreateBasePathMappingOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--rest-api-id")] string RestApiId
) : AwsOptions
{
    [CommandSwitch("--base-path")]
    public string? BasePath { get; set; }

    [CommandSwitch("--stage")]
    public string? Stage { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}