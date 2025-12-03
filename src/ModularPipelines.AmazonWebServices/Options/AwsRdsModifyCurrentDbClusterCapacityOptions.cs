using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "modify-current-db-cluster-capacity")]
public record AwsRdsModifyCurrentDbClusterCapacityOptions(
[property: CliOption("--db-cluster-identifier")] string DbClusterIdentifier
) : AwsOptions
{
    [CliOption("--capacity")]
    public int? Capacity { get; set; }

    [CliOption("--seconds-before-timeout")]
    public int? SecondsBeforeTimeout { get; set; }

    [CliOption("--timeout-action")]
    public string? TimeoutAction { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}