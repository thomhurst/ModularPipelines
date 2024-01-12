using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("recommender", "insight-type-config", "describe")]
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string InsightType { get; set; }

    [CommandSwitch("--location")]
    public string Location { get; set; }

    [CommandSwitch("--billing-account")]
    public string BillingAccount { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }
}
