using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("load", "test-run", "download-files")]
public record AzLoadTestRunDownloadFilesOptions(
[property: CommandSwitch("--load-test-resource")] string LoadTestResource,
[property: CommandSwitch("--path")] string Path,
[property: CommandSwitch("--test-run-id")] string TestRunId
) : AzOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--input")]
    public bool? Input { get; set; }

    [BooleanCommandSwitch("--log")]
    public bool? Log { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [BooleanCommandSwitch("--result")]
    public bool? Result { get; set; }
}

