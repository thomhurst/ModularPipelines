using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kms", "replicate-key")]
public record AwsKmsReplicateKeyOptions(
[property: CommandSwitch("--key-id")] string KeyId,
[property: CommandSwitch("--replica-region")] string ReplicaRegion
) : AwsOptions
{
    [CommandSwitch("--policy")]
    public string? Policy { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}