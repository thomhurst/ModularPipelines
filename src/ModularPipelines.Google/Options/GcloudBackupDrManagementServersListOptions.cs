using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("backup-dr", "management-servers", "list")]
public record GcloudBackupDrManagementServersListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}