using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functionapp", "deploy")]
public record AzFunctionappDeployOptions : AzOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliFlag("--clean")]
    public bool? Clean { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--ignore-stack")]
    public bool? IgnoreStack { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--restart")]
    public bool? Restart { get; set; }

    [CliOption("--slot")]
    public string? Slot { get; set; }

    [CliOption("--src-path")]
    public string? SrcPath { get; set; }

    [CliOption("--src-url")]
    public string? SrcUrl { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--target-path")]
    public string? TargetPath { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}