using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "draft", "setup-gh")]
public record AzAksDraftSetupGhOptions : AzOptions
{
    [CliOption("--app")]
    public string? App { get; set; }

    [CliOption("--gh-repo")]
    public string? GhRepo { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--provider")]
    public string? Provider { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }
}