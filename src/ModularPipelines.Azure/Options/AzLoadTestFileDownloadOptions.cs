using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("load", "test", "file", "download")]
public record AzLoadTestFileDownloadOptions(
[property: CommandSwitch("--file-name")] string FileName,
[property: CommandSwitch("--load-test-resource")] string LoadTestResource,
[property: CommandSwitch("--path")] string Path,
[property: CommandSwitch("--test-id")] string TestId
) : AzOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}