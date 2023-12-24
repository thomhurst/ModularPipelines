using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigateway", "update-domain-name")]
public record AwsApigatewayUpdateDomainNameOptions(
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--patch-operations")]
    public string[]? PatchOperations { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}