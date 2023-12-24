using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("events", "put-permission")]
public record AwsEventsPutPermissionOptions : AwsOptions
{
    [CommandSwitch("--event-bus-name")]
    public string? EventBusName { get; set; }

    [CommandSwitch("--action")]
    public string? Action { get; set; }

    [CommandSwitch("--principal")]
    public string? Principal { get; set; }

    [CommandSwitch("--statement-id")]
    public string? StatementId { get; set; }

    [CommandSwitch("--condition")]
    public string? Condition { get; set; }

    [CommandSwitch("--policy")]
    public string? Policy { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}