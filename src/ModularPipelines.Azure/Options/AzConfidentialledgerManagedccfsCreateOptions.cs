using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("confidentialledger", "managedccfs", "create")]
public record AzConfidentialledgerManagedccfsCreateOptions(
[property: CliOption("--members")] string Members,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--app-type")]
    public string? AppType { get; set; }

    [CliOption("--language-runtime")]
    public string? LanguageRuntime { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--node-count")]
    public int? NodeCount { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}