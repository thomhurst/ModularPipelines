using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearchserverless", "batch-get-effective-lifecycle-policy")]
public record AwsOpensearchserverlessBatchGetEffectiveLifecyclePolicyOptions(
[property: CommandSwitch("--resource-identifiers")] string[] ResourceIdentifiers
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}