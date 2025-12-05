using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivschat", "disconnect-user")]
public record AwsIvschatDisconnectUserOptions(
[property: CliOption("--room-identifier")] string RoomIdentifier,
[property: CliOption("--user-id")] string UserId
) : AwsOptions
{
    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}