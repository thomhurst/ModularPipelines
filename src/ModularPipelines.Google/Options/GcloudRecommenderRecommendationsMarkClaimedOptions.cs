using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("recommender", "recommendations", "mark-claimed")]
public record GcloudRecommenderRecommendationsMarkClaimedOptions : GcloudOptions
{
    public GcloudRecommenderRecommendationsMarkClaimedOptions(
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

    [PositionalArgument(Position = Position.BeforeSwitches)]
    public string Recommendation { get; set; }

    [CommandSwitch("--etag")]
    public string Etag { get; set; }

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

    [CommandSwitch("--state-metadata")]
    public IEnumerable<KeyValue>? StateMetadata { get; set; }
}
