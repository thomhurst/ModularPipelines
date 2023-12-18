using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "draft", "setup-gh")]
public record AzAksDraftSetupGhOptions : AzOptions
{
    [CommandSwitch("--app")]
    public string? App { get; set; }

    [CommandSwitch("--gh-repo")]
    public string? GhRepo { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--provider")]
    public string? Provider { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }
}

