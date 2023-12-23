using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "unshare-directory")]
public record AwsDsUnshareDirectoryOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--unshare-target")] string UnshareTarget
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}