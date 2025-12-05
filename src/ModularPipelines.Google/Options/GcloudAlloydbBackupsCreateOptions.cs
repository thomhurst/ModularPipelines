using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "backups", "create")]
public record GcloudAlloydbBackupsCreateOptions(
[property: CliArgument] string Backup,
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CliOption("--kms-location")]
    public string? KmsLocation { get; set; }

    [CliOption("--kms-project")]
    public string? KmsProject { get; set; }
}