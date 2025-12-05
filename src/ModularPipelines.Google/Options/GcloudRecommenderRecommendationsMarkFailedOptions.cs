using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recommender", "recommendations", "mark-failed")]
public record GcloudRecommenderRecommendationsMarkFailedOptions : GcloudOptions
{
    public GcloudRecommenderRecommendationsMarkFailedOptions(
        string recommendation,
        string etag,
        string location,
        string recommender,
        string billingAccount,
        string folder,
        string organization,
        string project
    )
    {
        Recommendation = recommendation;
        Etag = etag;
        Location = location;
        Recommender = recommender;
        BillingAccount = billingAccount;
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Recommendation { get; set; }

    [CliOption("--etag")]
    public string Etag { get; set; }

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

    [CliOption("--state-metadata")]
    public IEnumerable<KeyValue>? StateMetadata { get; set; }
}
