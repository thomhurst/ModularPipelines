using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "create-global-cluster")]
public record AwsRdsCreateGlobalClusterOptions : AwsOptions
{
    [CommandSwitch("--global-cluster-identifier")]
    public string? GlobalClusterIdentifier { get; set; }

    [CommandSwitch("--source-db-cluster-identifier")]
    public string? SourceDbClusterIdentifier { get; set; }

    [CommandSwitch("--engine")]
    public string? Engine { get; set; }

    [CommandSwitch("--engine-version")]
    public string? EngineVersion { get; set; }

    [CommandSwitch("--database-name")]
    public string? DatabaseName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}