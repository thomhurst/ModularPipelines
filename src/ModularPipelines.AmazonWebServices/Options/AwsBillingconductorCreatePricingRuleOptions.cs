using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billingconductor", "create-pricing-rule")]
public record AwsBillingconductorCreatePricingRuleOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--scope")] string Scope,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--modifier-percentage")]
    public double? ModifierPercentage { get; set; }

    [CommandSwitch("--service")]
    public string? Service { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--billing-entity")]
    public string? BillingEntity { get; set; }

    [CommandSwitch("--tiering")]
    public string? Tiering { get; set; }

    [CommandSwitch("--usage-type")]
    public string? UsageType { get; set; }

    [CommandSwitch("--operation")]
    public string? Operation { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}