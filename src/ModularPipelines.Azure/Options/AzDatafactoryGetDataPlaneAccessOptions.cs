using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "get-data-plane-access")]
public record AzDatafactoryGetDataPlaneAccessOptions : AzOptions
{
    [CommandSwitch("--access-resource-path")]
    public string? AccessResourcePath { get; set; }

    [CommandSwitch("--expire-time")]
    public string? ExpireTime { get; set; }

    [CommandSwitch("--factory-name")]
    public string? FactoryName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--permissions")]
    public string? Permissions { get; set; }

    [CommandSwitch("--profile-name")]
    public string? ProfileName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--start-time")]
    public string? StartTime { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}