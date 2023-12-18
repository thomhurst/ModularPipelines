using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("load", "test", "download-files")]
public record AzLoadTestDownloadFilesOptions(
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

