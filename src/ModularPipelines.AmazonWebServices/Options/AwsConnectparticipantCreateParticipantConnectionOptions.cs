using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectparticipant", "create-participant-connection")]
public record AwsConnectparticipantCreateParticipantConnectionOptions(
[property: CommandSwitch("--participant-token")] string ParticipantToken
) : AwsOptions
{
    [CommandSwitch("--type")]
    public string[]? Type { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}