using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "configure-factory-repo")]
public record AzDatafactoryConfigureFactoryRepoOptions(
[property: CommandSwitch("--factory-name")] string FactoryName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--factory-git-hub-configuration")]
    public string? FactoryGitHubConfiguration { get; set; }

    [CommandSwitch("--factory-resource-id")]
    public string? FactoryResourceId { get; set; }

    [CommandSwitch("--factory-vsts-configuration")]
    public string? FactoryVstsConfiguration { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}