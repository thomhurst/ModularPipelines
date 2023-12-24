using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("docdb", "create-global-cluster")]
public record AwsDocdbCreateGlobalClusterOptions(
[property: CommandSwitch("--global-cluster-identifier")] string GlobalClusterIdentifier
) : AwsOptions
{
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