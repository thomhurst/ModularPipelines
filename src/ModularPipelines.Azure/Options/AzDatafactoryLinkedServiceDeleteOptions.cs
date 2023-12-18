using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "linked-service", "delete")]
public record AzDatafactoryLinkedServiceDeleteOptions : AzOptions
{
    [CommandSwitch("--factory-name")]
    public string? FactoryName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--linked-service-name")]
    public string? LinkedServiceName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [BooleanCommandSwitch("--yes")]
    public bool? Yes { get; set; }
}