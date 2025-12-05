using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mediatailor", "delete-program")]
public record AwsMediatailorDeleteProgramOptions(
[property: CliOption("--channel-name")] string ChannelName,
[property: CliOption("--program-name")] string ProgramName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}