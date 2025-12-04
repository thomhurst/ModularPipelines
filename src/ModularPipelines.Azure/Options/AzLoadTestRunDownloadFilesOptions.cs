using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("load", "test-run", "download-files")]
public record AzLoadTestRunDownloadFilesOptions(
[property: CliOption("--load-test-resource")] string LoadTestResource,
[property: CliOption("--path")] string Path,
[property: CliOption("--test-run-id")] string TestRunId
) : AzOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliFlag("--input")]
    public bool? Input { get; set; }

    [CliFlag("--log")]
    public bool? Log { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliFlag("--result")]
    public bool? Result { get; set; }
}