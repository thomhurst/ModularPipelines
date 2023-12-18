using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datafactory", "linked-service", "create")]
public record AzDatafactoryLinkedServiceCreateOptions(
[property: CommandSwitch("--factory-name")] string FactoryName,
[property: CommandSwitch("--linked-service-name")] string LinkedServiceName,
[property: CommandSwitch("--properties")] string Properties,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }
}