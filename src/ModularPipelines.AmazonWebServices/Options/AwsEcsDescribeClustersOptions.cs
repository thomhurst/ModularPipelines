using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ecs", "describe-clusters")]
public record AwsEcsDescribeClustersOptions : AwsOptions
{
    [CommandSwitch("--clusters")]
    public string[]? Clusters { get; set; }

    [CommandSwitch("--include")]
    public string[]? Include { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}