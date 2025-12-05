using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fsx", "update-volume")]
public record AwsFsxUpdateVolumeOptions(
[property: CliOption("--volume-id")] string VolumeId
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--ontap-configuration")]
    public string? OntapConfiguration { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--open-zfs-configuration")]
    public string? OpenZfsConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}