using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "fs", "directory", "upload")]
public record AzStorageFsDirectoryUploadOptions(
[property: CommandSwitch("--file-system")] string FileSystem,
[property: CommandSwitch("--source")] string Source
) : AzOptions
{
    [CommandSwitch("--account-key")]
    public int? AccountKey { get; set; }

    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--connection-string")]
    public string? ConnectionString { get; set; }

    [CommandSwitch("--destination-path")]
    public string? DestinationPath { get; set; }

    [CommandSwitch("--recursive")]
    public string? Recursive { get; set; }

    [CommandSwitch("--sas-token")]
    public string? SasToken { get; set; }
}