using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quantum", "job", "submit")]
public record AzQuantumJobSubmitOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--target-id")] string TargetId,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions
{
    [CliOption("--entry-point")]
    public string? EntryPoint { get; set; }

    [CliOption("--job-input-file")]
    public string? JobInputFile { get; set; }

    [CliOption("--job-input-format")]
    public string? JobInputFormat { get; set; }

    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--job-output-format")]
    public string? JobOutputFormat { get; set; }

    [CliOption("--job-params")]
    public string? JobParams { get; set; }

    [CliFlag("--no-build")]
    public bool? NoBuild { get; set; }

    [CliOption("--project")]
    public string? Project { get; set; }

    [CliOption("--shots")]
    public string? Shots { get; set; }

    [CliOption("--storage")]
    public string? Storage { get; set; }

    [CliOption("--target-capability")]
    public string? TargetCapability { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? PROGRAMARGS { get; set; }
}