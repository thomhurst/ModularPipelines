using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medialive", "create-multiplex-program")]
public record AwsMedialiveCreateMultiplexProgramOptions(
[property: CommandSwitch("--multiplex-id")] string MultiplexId,
[property: CommandSwitch("--multiplex-program-settings")] string MultiplexProgramSettings,
[property: CommandSwitch("--program-name")] string ProgramName
) : AwsOptions
{
    [CommandSwitch("--request-id")]
    public string? RequestId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}