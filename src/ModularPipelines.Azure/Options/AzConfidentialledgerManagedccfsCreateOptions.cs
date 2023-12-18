using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("confidentialledger", "managedccfs", "create")]
public record AzConfidentialledgerManagedccfsCreateOptions(
[property: CommandSwitch("--members")] string Members,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--app-type")]
    public string? AppType { get; set; }

    [CommandSwitch("--language-runtime")]
    public string? LanguageRuntime { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--node-count")]
    public int? NodeCount { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

