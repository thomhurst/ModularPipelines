using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "webhook", "update")]
public record AzAcrWebhookUpdateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry
) : AzOptions
{
    [CliOption("--actions")]
    public string? Actions { get; set; }

    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--headers")]
    public string? Headers { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--uri")]
    public string? Uri { get; set; }
}