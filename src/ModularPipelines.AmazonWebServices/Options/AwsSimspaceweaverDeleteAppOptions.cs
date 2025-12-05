using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("simspaceweaver", "delete-app")]
public record AwsSimspaceweaverDeleteAppOptions(
[property: CliOption("--app")] string App,
[property: CliOption("--domain")] string Domain,
[property: CliOption("--simulation")] string Simulation
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}