using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune", "modify-global-cluster")]
public record AwsNeptuneModifyGlobalClusterOptions(
[property: CliOption("--global-cluster-identifier")] string GlobalClusterIdentifier
) : AwsOptions
{
    [CliOption("--new-global-cluster-identifier")]
    public string? NewGlobalClusterIdentifier { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}