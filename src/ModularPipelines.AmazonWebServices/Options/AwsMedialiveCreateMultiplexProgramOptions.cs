using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "create-multiplex-program")]
public record AwsMedialiveCreateMultiplexProgramOptions(
[property: CliOption("--multiplex-id")] string MultiplexId,
[property: CliOption("--multiplex-program-settings")] string MultiplexProgramSettings,
[property: CliOption("--program-name")] string ProgramName
) : AwsOptions
{
    [CliOption("--request-id")]
    public string? RequestId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}