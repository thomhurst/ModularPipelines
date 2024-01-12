using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("recommender", "insight-type-config", "update")]
public record GcloudRecommenderInsightTypeConfigUpdateOptions : GcloudOptions
{
    public GcloudRecommenderInsightTypeConfigUpdateOptions(
        string insightType,
        string etag,
        string location,
        string billingAccount,
        string organization,
        string project
    )
    {
        InsightType = insightType;
        Etag = etag;
        Location = location;
        BillingAccount = billingAccount;
        Organization = organization;
        Project = project;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string InsightType { get; set; }

    [CommandSwitch("--etag")]
    public string Etag { get; set; }

    [CommandSwitch("--location")]
    public string Location { get; set; }

    [CommandSwitch("--billing-account")]
    public string BillingAccount { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }

    [CommandSwitch("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [CommandSwitch("--config-file")]
    public string? ConfigFile { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--validate-only")]
    public bool? ValidateOnly { get; set; }
}
