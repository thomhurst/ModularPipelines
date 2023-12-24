using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "update-multiplex-program")]
public record AwsMedialiveUpdateMultiplexProgramOptions(
[property: CommandSwitch("--multiplex-id")] string MultiplexId,
[property: CommandSwitch("--program-name")] string ProgramName
) : AwsOptions
{
    [CommandSwitch("--multiplex-program-settings")]
    public string? MultiplexProgramSettings { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}