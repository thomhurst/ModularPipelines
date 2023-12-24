using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apigatewayv2", "update-domain-name")]
public record AwsApigatewayv2UpdateDomainNameOptions(
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--domain-name-configurations")]
    public string[]? DomainNameConfigurations { get; set; }

    [CommandSwitch("--mutual-tls-authentication")]
    public string? MutualTlsAuthentication { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}