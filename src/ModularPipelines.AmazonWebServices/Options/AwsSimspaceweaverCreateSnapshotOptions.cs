using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("simspaceweaver", "create-snapshot")]
public record AwsSimspaceweaverCreateSnapshotOptions(
[property: CliOption("--destination")] string Destination,
[property: CliOption("--simulation")] string Simulation
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}