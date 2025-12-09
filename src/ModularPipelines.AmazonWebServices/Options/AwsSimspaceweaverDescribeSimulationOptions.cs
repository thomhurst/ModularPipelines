using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("simspaceweaver", "describe-simulation")]
public record AwsSimspaceweaverDescribeSimulationOptions(
[property: CliOption("--simulation")] string Simulation
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}