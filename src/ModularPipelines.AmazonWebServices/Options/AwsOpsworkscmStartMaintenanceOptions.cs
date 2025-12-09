using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("opsworkscm", "start-maintenance")]
public record AwsOpsworkscmStartMaintenanceOptions(
[property: CliOption("--server-name")] string ServerName
) : AwsOptions
{
    [CliOption("--engine-attributes")]
    public string[]? EngineAttributes { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}