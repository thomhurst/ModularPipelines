using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storagegateway", "refresh-cache")]
public record AwsStoragegatewayRefreshCacheOptions(
[property: CommandSwitch("--file-share-arn")] string FileShareArn
) : AwsOptions
{
    [CommandSwitch("--folder-list")]
    public string[]? FolderList { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}