using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("events", "put-permission")]
public record AwsEventsPutPermissionOptions : AwsOptions
{
    [CliOption("--event-bus-name")]
    public string? EventBusName { get; set; }

    [CliOption("--action")]
    public string? Action { get; set; }

    [CliOption("--principal")]
    public string? Principal { get; set; }

    [CliOption("--statement-id")]
    public string? StatementId { get; set; }

    [CliOption("--condition")]
    public string? Condition { get; set; }

    [CliOption("--policy")]
    public string? Policy { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}