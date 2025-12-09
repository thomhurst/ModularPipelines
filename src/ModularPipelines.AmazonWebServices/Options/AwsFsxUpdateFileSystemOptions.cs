using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("fsx", "update-file-system")]
public record AwsFsxUpdateFileSystemOptions(
[property: CliOption("--file-system-id")] string FileSystemId
) : AwsOptions
{
    [CliOption("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CliOption("--storage-capacity")]
    public int? StorageCapacity { get; set; }

    [CliOption("--windows-configuration")]
    public string? WindowsConfiguration { get; set; }

    [CliOption("--lustre-configuration")]
    public string? LustreConfiguration { get; set; }

    [CliOption("--ontap-configuration")]
    public string? OntapConfiguration { get; set; }

    [CliOption("--open-zfs-configuration")]
    public string? OpenZfsConfiguration { get; set; }

    [CliOption("--storage-type")]
    public string? StorageType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}