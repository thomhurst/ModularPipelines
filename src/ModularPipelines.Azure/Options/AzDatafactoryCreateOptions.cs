using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datafactory", "create")]
public record AzDatafactoryCreateOptions(
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--factory-git-hub-configuration")]
    public string? FactoryGitHubConfiguration { get; set; }

    [CliOption("--factory-vsts-configuration")]
    public string? FactoryVstsConfiguration { get; set; }

    [CliOption("--global-parameters")]
    public string? GlobalParameters { get; set; }

    [CliOption("--if-match")]
    public string? IfMatch { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}