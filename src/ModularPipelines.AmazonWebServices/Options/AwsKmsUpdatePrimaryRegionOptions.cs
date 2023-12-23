using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "update-primary-region")]
public record AwsKmsUpdatePrimaryRegionOptions(
[property: CommandSwitch("--key-id")] string KeyId,
[property: CommandSwitch("--primary-region")] string PrimaryRegion
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}