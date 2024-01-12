using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("recommender", "insights", "list")]
public record GcloudRecommenderInsightsListOptions : GcloudOptions
{
    public GcloudRecommenderInsightsListOptions(
        string insightType,
        string location,
        string billingAccount,
        string folder,
        string organization,
        string project
    )
    {
        InsightType = insightType;
        Location = location;
        BillingAccount = billingAccount;
        Folder = folder;
        Organization = organization;
        Project = project;
    }

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
}
