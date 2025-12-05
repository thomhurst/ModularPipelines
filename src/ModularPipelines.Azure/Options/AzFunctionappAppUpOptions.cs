using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("functionapp", "app", "up")]
public record AzFunctionappAppUpOptions : AzOptions
{
    [CliOption("--app-name")]
    public string? AppName { get; set; }

    [CliOption("--branch-name")]
    public string? BranchName { get; set; }

    [CliFlag("--do-not-wait")]
    public bool? DoNotWait { get; set; }

    [CliOption("--repository")]
    public string? Repository { get; set; }
}