using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("notebooks", "runtimes", "migrate")]
public record GcloudNotebooksRuntimesMigrateOptions(
[property: CliArgument] string Runtime,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--post-startup-script-option")]
    public string? PostStartupScriptOption { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--network")]
    public string? Network { get; set; }

    [CliOption("--subnet")]
    public string? Subnet { get; set; }

    [CliOption("--subnet-region")]
    public string? SubnetRegion { get; set; }
}