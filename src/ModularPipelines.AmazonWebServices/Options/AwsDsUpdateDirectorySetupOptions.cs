using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "update-directory-setup")]
public record AwsDsUpdateDirectorySetupOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--update-type")] string UpdateType
) : AwsOptions
{
    [CommandSwitch("--os-update-settings")]
    public string? OsUpdateSettings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}