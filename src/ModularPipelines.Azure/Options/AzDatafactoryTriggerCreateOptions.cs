using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datafactory", "trigger", "create")]
public record AzDatafactoryTriggerCreateOptions(
[property: CliOption("--factory-name")] string FactoryName,
[property: CliOption("--name")] string Name,
[property: CliOption("--properties")] string Properties,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--if-match")]
    public string? IfMatch { get; set; }
}