using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "configure-factory-repo")]
public record AzDatafactoryConfigureFactoryRepoOptions : AzOptions
{
    [CliOption("--factory-git-hub-configuration")]
    public string? FactoryGitHubConfiguration { get; set; }

    [CliOption("--factory-resource-id")]
    public string? FactoryResourceId { get; set; }

    [CliOption("--factory-vsts-configuration")]
    public string? FactoryVstsConfiguration { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}