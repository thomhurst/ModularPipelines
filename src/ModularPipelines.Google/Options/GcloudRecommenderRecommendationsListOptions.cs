using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("recommender", "recommendations", "list")]
public record GcloudRecommenderRecommendationsListOptions : GcloudOptions
{
    public GcloudRecommenderRecommendationsListOptions(
        string location,
        string recommender,
        string billingAccount,
        string folder,
        string organization,
        string project
    )
    {
        Location = location;
        Recommender = recommender;
        BillingAccount = billingAccount;
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [CommandSwitch("--location")]
    public string Location { get; set; }

    [CommandSwitch("--recommender")]
    public string Recommender { get; set; }

    [CommandSwitch("--billing-account")]
    public string BillingAccount { get; set; }

    [CommandSwitch("--folder")]
    public string Folder { get; set; }

    [CommandSwitch("--organization")]
    public string Organization { get; set; }
}
