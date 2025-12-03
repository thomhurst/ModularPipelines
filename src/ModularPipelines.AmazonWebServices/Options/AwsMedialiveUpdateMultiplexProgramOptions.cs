using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "update-multiplex-program")]
public record AwsMedialiveUpdateMultiplexProgramOptions(
[property: CliOption("--multiplex-id")] string MultiplexId,
[property: CliOption("--program-name")] string ProgramName
) : AwsOptions
{
    [CliOption("--multiplex-program-settings")]
    public string? MultiplexProgramSettings { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}