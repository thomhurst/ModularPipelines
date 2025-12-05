using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recommender", "recommendations", "list")]
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

    [CliOption("--location")]
    public string Location { get; set; }

    [CliOption("--recommender")]
    public string Recommender { get; set; }

    [CliOption("--billing-account")]
    public string BillingAccount { get; set; }

    [CliOption("--folder")]
    public string Folder { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }
}
