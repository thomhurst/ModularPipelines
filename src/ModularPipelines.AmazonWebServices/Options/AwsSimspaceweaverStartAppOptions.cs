using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("simspaceweaver", "start-app")]
public record AwsSimspaceweaverStartAppOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--simulation")] string Simulation
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--launch-overrides")]
    public string? LaunchOverrides { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}