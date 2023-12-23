using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearchserverless", "batch-get-lifecycle-policy")]
public record AwsOpensearchserverlessBatchGetLifecyclePolicyOptions(
[property: CommandSwitch("--identifiers")] string[] Identifiers
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}