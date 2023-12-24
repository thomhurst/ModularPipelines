using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devicefarm", "create-vpce-configuration")]
public record AwsDevicefarmCreateVpceConfigurationOptions(
[property: CommandSwitch("--vpce-configuration-name")] string VpceConfigurationName,
[property: CommandSwitch("--vpce-service-name")] string VpceServiceName,
[property: CommandSwitch("--service-dns-name")] string ServiceDnsName
) : AwsOptions
{
    [CommandSwitch("--vpce-configuration-description")]
    public string? VpceConfigurationDescription { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}