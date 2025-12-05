using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("recommender", "recommender-config", "update")]
public record GcloudRecommenderRecommenderConfigUpdateOptions : GcloudOptions
{
    public GcloudRecommenderRecommenderConfigUpdateOptions(
        string recommender,
        string etag,
        string location,
        string billingAccount,
        string organization,
        string project
    )
    {
        Recommender = recommender;
        Etag = etag;
        Location = location;
        BillingAccount = billingAccount;
        Organization = organization;
        Project = project;
    }

    [CliArgument(Placement = ArgumentPlacement.BeforeOptions)]
    public string Recommender { get; set; }

    [CliOption("--etag")]
    public string Etag { get; set; }

    [CliOption("--location")]
    public string Location { get; set; }

    [CliOption("--billing-account")]
    public string BillingAccount { get; set; }

    [CliOption("--organization")]
    public string Organization { get; set; }

    [CliOption("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [CliOption("--config-file")]
    public string? ConfigFile { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--validate-only")]
    public bool? ValidateOnly { get; set; }
}
