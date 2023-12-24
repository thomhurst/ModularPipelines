using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("simspaceweaver", "create-snapshot")]
public record AwsSimspaceweaverCreateSnapshotOptions(
[property: CommandSwitch("--destination")] string Destination,
[property: CommandSwitch("--simulation")] string Simulation
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}