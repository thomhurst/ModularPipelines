using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("events", "create-partner-event-source")]
public record AwsEventsCreatePartnerEventSourceOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--account")] string Account
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}