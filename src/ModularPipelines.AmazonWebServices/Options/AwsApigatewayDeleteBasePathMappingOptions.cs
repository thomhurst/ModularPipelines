using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "delete-base-path-mapping")]
public record AwsApigatewayDeleteBasePathMappingOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--base-path")] string BasePath
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}