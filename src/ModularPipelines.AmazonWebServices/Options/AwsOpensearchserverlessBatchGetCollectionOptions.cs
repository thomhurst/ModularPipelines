using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opensearchserverless", "batch-get-collection")]
public record AwsOpensearchserverlessBatchGetCollectionOptions : AwsOptions
{
    [CommandSwitch("--ids")]
    public string[]? Ids { get; set; }

    [CommandSwitch("--names")]
    public string[]? Names { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}