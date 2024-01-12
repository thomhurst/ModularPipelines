using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("recommender", "insights", "mark-accepted")]
public record GcloudRecommenderInsightsMarkAcceptedOptions : GcloudOptions
{
    public GcloudRecommenderInsightsMarkAcceptedOptions(
        string insight,
        string etag,
        string insightType,
        string location,
        string billingAccount,
        string folder,
        string organization,
        string project
    )
    {
        Insight = insight;
        Etag = etag;
        InsightType = insightType;
        Location = location;
        BillingAccount = billingAccount;
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Insight { get; set; }

    [CommandSwitch("--etag")]
    public string Etag { get; set; }

    [CommandSwitch("--insight-type")]
    public string InsightType { get; set; }

    [CommandSwitch("--location")]
    public string Location { get; set; }

    [CommandSwitch("--billing-account")]
    public string BillingAccount { get; set; }

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }

    [CommandSwitch("--state-metadata")]
    public IEnumerable<KeyValue>? StateMetadata { get; set; }
}
