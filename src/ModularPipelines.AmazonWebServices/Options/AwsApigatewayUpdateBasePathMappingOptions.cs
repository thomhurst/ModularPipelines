using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "update-base-path-mapping")]
public record AwsApigatewayUpdateBasePathMappingOptions(
[property: CommandSwitch("--domain-name")] string DomainName,
[property: CommandSwitch("--base-path")] string BasePath
) : AwsOptions
{
    [CommandSwitch("--patch-operations")]
    public string[]? PatchOperations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}