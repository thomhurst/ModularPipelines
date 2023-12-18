using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tsi", "reference-data-set", "create")]
public record AzTsiReferenceDataSetCreateOptions(
[property: CommandSwitch("--environment-name")] string EnvironmentName,
[property: CommandSwitch("--key-properties")] string KeyProperties,
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--comparison-behavior")]
    public string? ComparisonBehavior { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}