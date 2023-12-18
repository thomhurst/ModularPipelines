using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "app", "up")]
public record AzFunctionappAppUpOptions : AzOptions
{
    [CommandSwitch("--app-name")]
    public string? AppName { get; set; }

    [CommandSwitch("--branch-name")]
    public string? BranchName { get; set; }

    [BooleanCommandSwitch("--do-not-wait")]
    public bool? DoNotWait { get; set; }

    [CommandSwitch("--repository")]
    public string? Repository { get; set; }
}