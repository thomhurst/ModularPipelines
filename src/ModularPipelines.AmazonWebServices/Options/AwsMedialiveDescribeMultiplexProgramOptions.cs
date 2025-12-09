using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medialive", "describe-multiplex-program")]
public record AwsMedialiveDescribeMultiplexProgramOptions(
[property: CliOption("--multiplex-id")] string MultiplexId,
[property: CliOption("--program-name")] string ProgramName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}