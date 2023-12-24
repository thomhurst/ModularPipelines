using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "delete-multiplex-program")]
public record AwsMedialiveDeleteMultiplexProgramOptions(
[property: CommandSwitch("--multiplex-id")] string MultiplexId,
[property: CommandSwitch("--program-name")] string ProgramName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}