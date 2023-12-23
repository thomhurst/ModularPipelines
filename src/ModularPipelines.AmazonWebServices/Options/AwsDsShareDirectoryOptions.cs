using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ds", "share-directory")]
public record AwsDsShareDirectoryOptions(
[property: CommandSwitch("--directory-id")] string DirectoryId,
[property: CommandSwitch("--share-target")] string ShareTarget,
[property: CommandSwitch("--share-method")] string ShareMethod
) : AwsOptions
{
    [CommandSwitch("--share-notes")]
    public string? ShareNotes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}