using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("recommender", "recommender-config", "describe")]
public record GcloudRecommenderRecommenderConfigDescribeOptions : GcloudOptions
{
    public GcloudRecommenderRecommenderConfigDescribeOptions(
        string recommender,
        string location,
        string billingAccount,
        string organization,
        string project
    )
    {
        Recommender = recommender;
        Location = location;
        BillingAccount = billingAccount;
        Organization = organization;
        Project = project;
    }

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Recommender { get; set; }

    [CommandSwitch("--location")]
    public string Location { get; set; }

    [CommandSwitch("--billing-account")]
    public string BillingAccount { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }
}
