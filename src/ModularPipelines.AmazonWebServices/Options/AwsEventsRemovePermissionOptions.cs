using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("events", "remove-permission")]
public record AwsEventsRemovePermissionOptions : AwsOptions
{
    [CommandSwitch("--statement-id")]
    public string? StatementId { get; set; }

    [CommandSwitch("--event-bus-name")]
    public string? EventBusName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}