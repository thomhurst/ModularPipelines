using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "disconnect-custom-key-store")]
public record AwsKmsDisconnectCustomKeyStoreOptions(
[property: CommandSwitch("--custom-key-store-id")] string CustomKeyStoreId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}