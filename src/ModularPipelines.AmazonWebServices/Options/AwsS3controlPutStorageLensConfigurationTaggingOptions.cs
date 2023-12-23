using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("s3control", "put-storage-lens-configuration-tagging")]
public record AwsS3controlPutStorageLensConfigurationTaggingOptions(
[property: CommandSwitch("--config-id")] string ConfigId,
[property: CommandSwitch("--account-id")] string AccountId,
[property: CommandSwitch("--tags")] string[] Tags
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}