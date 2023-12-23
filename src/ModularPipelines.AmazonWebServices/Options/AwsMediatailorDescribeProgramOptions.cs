using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mediatailor", "describe-program")]
public record AwsMediatailorDescribeProgramOptions(
[property: CommandSwitch("--channel-name")] string ChannelName,
[property: CommandSwitch("--program-name")] string ProgramName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}