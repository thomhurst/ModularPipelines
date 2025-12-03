using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "jobs", "run")]
public record GcloudTransferJobsRunOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliFlag("--no-async")]
    public bool? NoAsync { get; set; }
}