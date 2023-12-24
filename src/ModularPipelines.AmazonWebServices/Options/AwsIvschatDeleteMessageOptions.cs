using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ivschat", "delete-message")]
public record AwsIvschatDeleteMessageOptions(
[property: CommandSwitch("--id")] string Id,
[property: CommandSwitch("--room-identifier")] string RoomIdentifier
) : AwsOptions
{
    [CommandSwitch("--reason")]
    public string? Reason { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}