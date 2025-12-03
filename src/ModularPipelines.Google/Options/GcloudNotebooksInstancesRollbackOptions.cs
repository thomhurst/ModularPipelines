using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "instances", "rollback")]
public record GcloudNotebooksInstancesRollbackOptions(
[property: CliArgument] string Instance,
[property: CliArgument] string Location,
[property: CliOption("--target-snapshot")] string TargetSnapshot
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}