using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recommender", "insights", "list")]
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

    [CliOption("--insight-type")]
    public string InsightType { get; set; }

    [CliOption("--location")]
    public string Location { get; set; }

    [CliOption("--billing-account")]
    public string BillingAccount { get; set; }

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }
}
