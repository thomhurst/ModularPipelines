using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("confluent", "organization", "create")]
public record AzConfluentOrganizationCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--plan-id")] string PlanId,
[property: CliOption("--plan-name")] string PlanName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--term-unit")] string TermUnit
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--offer-id")]
    public string? OfferId { get; set; }

    [CliOption("--publisher-id")]
    public string? PublisherId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}