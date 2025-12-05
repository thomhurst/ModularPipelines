using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recommender", "insights", "describe")]
public record GcloudRecommenderInsightsDescribeOptions : GcloudOptions
{
    public GcloudRecommenderInsightsDescribeOptions(
        string insight,
        string insightType,
        string location,
        string billingAccount,
        string folder,
        string organization,
        string project
    )
    {
        Insight = insight;
        InsightType = insightType;
        Location = location;
        BillingAccount = billingAccount;
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Insight { get; set; }

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
