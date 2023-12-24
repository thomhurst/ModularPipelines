using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fsx", "update-file-system")]
public record AwsFsxUpdateFileSystemOptions(
[property: CommandSwitch("--file-system-id")] string FileSystemId
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--storage-capacity")]
    public int? StorageCapacity { get; set; }

    [CommandSwitch("--windows-configuration")]
    public string? WindowsConfiguration { get; set; }

    [CommandSwitch("--lustre-configuration")]
    public string? LustreConfiguration { get; set; }

    [CommandSwitch("--ontap-configuration")]
    public string? OntapConfiguration { get; set; }

    [CommandSwitch("--open-zfs-configuration")]
    public string? OpenZfsConfiguration { get; set; }

    [CommandSwitch("--storage-type")]
    public string? StorageType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}