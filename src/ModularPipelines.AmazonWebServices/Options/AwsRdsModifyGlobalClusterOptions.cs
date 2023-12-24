using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "modify-global-cluster")]
public record AwsRdsModifyGlobalClusterOptions : AwsOptions
{
    [CommandSwitch("--global-cluster-identifier")]
    public string? GlobalClusterIdentifier { get; set; }

    [CommandSwitch("--new-global-cluster-identifier")]
    public string? NewGlobalClusterIdentifier { get; set; }

    [CommandSwitch("--engine-version")]
    public string? EngineVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}