using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "remove-permission")]
public record AwsEventsRemovePermissionOptions : AwsOptions
{
    [CliOption("--statement-id")]
    public string? StatementId { get; set; }

    [CliOption("--event-bus-name")]
    public string? EventBusName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}