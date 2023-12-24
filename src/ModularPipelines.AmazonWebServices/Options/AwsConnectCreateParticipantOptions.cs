using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "create-participant")]
public record AwsConnectCreateParticipantOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--contact-id")] string ContactId,
[property: CommandSwitch("--participant-details")] string ParticipantDetails
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}