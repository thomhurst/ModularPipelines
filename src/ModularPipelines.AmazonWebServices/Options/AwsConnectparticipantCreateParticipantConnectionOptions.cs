using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectparticipant", "create-participant-connection")]
public record AwsConnectparticipantCreateParticipantConnectionOptions(
[property: CliOption("--participant-token")] string ParticipantToken
) : AwsOptions
{
    [CliOption("--type")]
    public string[]? Type { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}