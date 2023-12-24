using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "get-base-path-mapping")]
public record AwsApigatewayGetBasePathMappingOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--base-path")] string BasePath
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}