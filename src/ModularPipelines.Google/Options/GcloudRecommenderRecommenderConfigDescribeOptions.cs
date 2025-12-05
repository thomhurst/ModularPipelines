using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recommender", "recommender-config", "describe")]
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

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Recommender { get; set; }

    [CliOption("--location")]
    public string Location { get; set; }

    [CliOption("--billing-account")]
    public string BillingAccount { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }
}
