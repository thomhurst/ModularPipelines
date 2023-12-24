using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "remove-from-global-cluster")]
public record AwsRdsRemoveFromGlobalClusterOptions : AwsOptions
{
    [CommandSwitch("--global-cluster-identifier")]
    public string? GlobalClusterIdentifier { get; set; }

    [CommandSwitch("--db-cluster-identifier")]
    public string? DbClusterIdentifier { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}