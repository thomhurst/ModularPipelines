using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recommender", "insights", "mark-accepted")]
public record GcloudRecommenderInsightsMarkAcceptedOptions : GcloudOptions
{
    public GcloudRecommenderInsightsMarkAcceptedOptions(
        string insight,
        string etag,
        string insightType,
        string location,
        string billingAccount,
        string folder,
        string organization,
        string project
    )
    {
        Insight = insight;
        Etag = etag;
        InsightType = insightType;
        Location = location;
        BillingAccount = billingAccount;
        Folder = folder;
        Organization = organization;
        Project = project;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Insight { get; set; }

    [CliOption("--etag")]
    public string Etag { get; set; }

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

    [CliOption("--state-metadata")]
    public IEnumerable<KeyValue>? StateMetadata { get; set; }
}
