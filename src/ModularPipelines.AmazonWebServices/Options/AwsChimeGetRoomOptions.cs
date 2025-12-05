using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("chime", "get-room")]
public record AwsChimeGetRoomOptions(
[property: CliOption("--account-id")] string AccountId,
[property: CliOption("--room-id")] string RoomId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}