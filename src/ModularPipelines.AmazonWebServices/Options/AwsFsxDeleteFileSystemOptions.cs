using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fsx", "delete-file-system")]
public record AwsFsxDeleteFileSystemOptions(
[property: CommandSwitch("--file-system-id")] string FileSystemId
) : AwsOptions
{
    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--windows-configuration")]
    public string? WindowsConfiguration { get; set; }

    [CommandSwitch("--lustre-configuration")]
    public string? LustreConfiguration { get; set; }

    [CommandSwitch("--open-zfs-configuration")]
    public string? OpenZfsConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}