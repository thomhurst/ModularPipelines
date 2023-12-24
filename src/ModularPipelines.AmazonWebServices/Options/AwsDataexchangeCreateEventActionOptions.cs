using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataexchange", "create-event-action")]
public record AwsDataexchangeCreateEventActionOptions(
[property: CommandSwitch("--action")] string Action,
[property: CommandSwitch("--event")] string Event
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}