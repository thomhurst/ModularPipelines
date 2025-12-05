using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recommender", "insight-type-config", "describe")]
public record GcloudRecommenderInsightTypeConfigDescribeOptions : GcloudOptions
{
    public GcloudRecommenderInsightTypeConfigDescribeOptions(
        string insightType,
        string location,
        string billingAccount,
        string organization,
        string project
    )
    {
        InsightType = insightType;
        Location = location;
        BillingAccount = billingAccount;
        Organization = organization;
        Project = project;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string InsightType { get; set; }

    [CliOption("--location")]
    public string Location { get; set; }

    [CliOption("--billing-account")]
    public string BillingAccount { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }
}
