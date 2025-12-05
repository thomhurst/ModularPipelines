using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("simspaceweaver", "start-app")]
public record AwsSimspaceweaverStartAppOptions(
[property: CliOption("--domain")] string Domain,
[property: CliOption("--name")] string Name,
[property: CliOption("--simulation")] string Simulation
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--launch-overrides")]
    public string? LaunchOverrides { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}