using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "webhook", "create")]
public record AzAcrWebhookCreateOptions(
[property: CliOption("--actions")] string Actions,
[property: CliOption("--name")] string Name,
[property: CliOption("--registry")] string Registry,
[property: CliOption("--uri")] string Uri
) : AzOptions
{
    [CliOption("--headers")]
    public string? Headers { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}