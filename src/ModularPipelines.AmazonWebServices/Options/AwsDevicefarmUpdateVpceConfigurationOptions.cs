using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devicefarm", "update-vpce-configuration")]
public record AwsDevicefarmUpdateVpceConfigurationOptions(
[property: CliOption("--arn")] string Arn
) : AwsOptions
{
    [CliOption("--vpce-configuration-name")]
    public string? VpceConfigurationName { get; set; }

    [CliOption("--vpce-service-name")]
    public string? VpceServiceName { get; set; }

    [CliOption("--service-dns-name")]
    public string? ServiceDnsName { get; set; }

    [CliOption("--vpce-configuration-description")]
    public string? VpceConfigurationDescription { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}