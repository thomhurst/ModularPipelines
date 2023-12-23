using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "modify-current-db-cluster-capacity")]
public record AwsRdsModifyCurrentDbClusterCapacityOptions(
[property: CommandSwitch("--db-cluster-identifier")] string DbClusterIdentifier
) : AwsOptions
{
    [CommandSwitch("--capacity")]
    public int? Capacity { get; set; }

    [CommandSwitch("--seconds-before-timeout")]
    public int? SecondsBeforeTimeout { get; set; }

    [CommandSwitch("--timeout-action")]
    public string? TimeoutAction { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}