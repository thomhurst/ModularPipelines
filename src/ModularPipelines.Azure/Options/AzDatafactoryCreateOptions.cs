using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "create")]
public record AzDatafactoryCreateOptions(
[property: CommandSwitch("--factory-name")] string FactoryName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--factory-git-hub-configuration")]
    public string? FactoryGitHubConfiguration { get; set; }

    [CommandSwitch("--factory-vsts-configuration")]
    public string? FactoryVstsConfiguration { get; set; }

    [CommandSwitch("--global-parameters")]
    public string? GlobalParameters { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--public-network-access")]
    public string? PublicNetworkAccess { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}