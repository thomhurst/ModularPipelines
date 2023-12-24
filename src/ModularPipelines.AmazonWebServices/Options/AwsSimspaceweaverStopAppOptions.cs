using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("simspaceweaver", "stop-app")]
public record AwsSimspaceweaverStopAppOptions(
[property: CommandSwitch("--app")] string App,
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--simulation")] string Simulation
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}