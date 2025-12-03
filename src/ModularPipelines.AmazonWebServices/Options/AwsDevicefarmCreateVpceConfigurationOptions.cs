using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devicefarm", "create-vpce-configuration")]
public record AwsDevicefarmCreateVpceConfigurationOptions(
[property: CliOption("--vpce-configuration-name")] string VpceConfigurationName,
[property: CliOption("--vpce-service-name")] string VpceServiceName,
[property: CliOption("--service-dns-name")] string ServiceDnsName
) : AwsOptions
{
    [CliOption("--vpce-configuration-description")]
    public string? VpceConfigurationDescription { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}