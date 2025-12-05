using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fsx", "create-volume")]
public record AwsFsxCreateVolumeOptions(
[property: CliOption("--volume-type")] string VolumeType,
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--ontap-configuration")]
    public string? OntapConfiguration { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--open-zfs-configuration")]
    public string? OpenZfsConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}