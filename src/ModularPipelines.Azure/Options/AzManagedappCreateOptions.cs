using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("managedapp", "create")]
public record AzManagedappCreateOptions(
[property: CliOption("--kind")] string Kind,
[property: CliOption("--managed-rg-id")] string ManagedRgId,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--managedapp-definition-id")]
    public string? ManagedappDefinitionId { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--plan-name")]
    public string? PlanName { get; set; }

    [CliOption("--plan-product")]
    public string? PlanProduct { get; set; }

    [CliOption("--plan-publisher")]
    public string? PlanPublisher { get; set; }

    [CliOption("--plan-version")]
    public string? PlanVersion { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}