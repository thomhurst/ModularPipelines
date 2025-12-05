using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivschat", "delete-message")]
public record AwsIvschatDeleteMessageOptions(
[property: CliOption("--id")] string Id,
[property: CliOption("--room-identifier")] string RoomIdentifier
) : AwsOptions
{
    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}