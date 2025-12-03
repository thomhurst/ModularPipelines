using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "migration-jobs", "generate-ssh-script")]
public record GcloudDatabaseMigrationMigrationJobsGenerateSshScriptOptions(
[property: CliArgument] string MigrationJob,
[property: CliArgument] string Region,
[property: CliOption("--vm")] string Vm,
[property: CliOption("--vm-zone")] string VmZone,
[property: CliOption("--subnet")] string Subnet,
[property: CliOption("--vm-machine-type")] string VmMachineType,
[property: CliOption("--vm-zone-create")] string VmZoneCreate
) : GcloudOptions
{
    [CliOption("--vm-port")]
    public string? VmPort { get; set; }
}