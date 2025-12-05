using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workbench", "instances", "rollback")]
public record GcloudWorkbenchInstancesRollbackOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Location,
[property: CliOption("--target-snapshot")] string TargetSnapshot
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}