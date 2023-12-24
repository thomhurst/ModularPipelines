using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediastore", "put-lifecycle-policy")]
public record AwsMediastorePutLifecyclePolicyOptions(
[property: CommandSwitch("--container-name")] string ContainerName,
[property: CommandSwitch("--lifecycle-policy")] string LifecyclePolicy
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}