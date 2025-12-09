using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("billingconductor", "create-pricing-rule")]
public record AwsBillingconductorCreatePricingRuleOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--scope")] string Scope,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--modifier-percentage")]
    public double? ModifierPercentage { get; set; }

    [CliOption("--service")]
    public string? Service { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--billing-entity")]
    public string? BillingEntity { get; set; }

    [CliOption("--tiering")]
    public string? Tiering { get; set; }

    [CliOption("--usage-type")]
    public string? UsageType { get; set; }

    [CliOption("--operation")]
    public string? Operation { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}