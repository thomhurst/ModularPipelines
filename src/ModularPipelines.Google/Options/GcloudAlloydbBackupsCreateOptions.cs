using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("alloydb", "backups", "create")]
public record GcloudAlloydbBackupsCreateOptions(
[property: PositionalArgument] string Backup,
[property: CommandSwitch("--cluster")] string Cluster,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }
}