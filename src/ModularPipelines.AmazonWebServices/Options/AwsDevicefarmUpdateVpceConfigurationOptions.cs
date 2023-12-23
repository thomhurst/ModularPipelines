using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devicefarm", "update-vpce-configuration")]
public record AwsDevicefarmUpdateVpceConfigurationOptions(
[property: CommandSwitch("--arn")] string Arn
) : AwsOptions
{
    [CommandSwitch("--vpce-configuration-name")]
    public string? VpceConfigurationName { get; set; }

    [CommandSwitch("--vpce-service-name")]
    public string? VpceServiceName { get; set; }

    [CommandSwitch("--service-dns-name")]
    public string? ServiceDnsName { get; set; }

    [CommandSwitch("--vpce-configuration-description")]
    public string? VpceConfigurationDescription { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}