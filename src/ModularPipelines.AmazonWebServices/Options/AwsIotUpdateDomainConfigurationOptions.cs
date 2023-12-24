using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "update-domain-configuration")]
public record AwsIotUpdateDomainConfigurationOptions(
[property: CommandSwitch("--domain-configuration-name")] string DomainConfigurationName
) : AwsOptions
{
    [CommandSwitch("--authorizer-config")]
    public string? AuthorizerConfig { get; set; }

    [CommandSwitch("--domain-configuration-status")]
    public string? DomainConfigurationStatus { get; set; }

    [CommandSwitch("--tls-config")]
    public string? TlsConfig { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}