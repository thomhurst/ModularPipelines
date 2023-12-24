using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "create-storage-lens-group")]
public record AwsS3controlCreateStorageLensGroupOptions(
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--storage-lens-group")] string StorageLensGroup
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}