using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "put-storage-lens-configuration")]
public record AwsS3controlPutStorageLensConfigurationOptions(
[property: CommandSwitch("--config-id")] string ConfigId,
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--storage-lens-configuration")] string StorageLensConfiguration
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}