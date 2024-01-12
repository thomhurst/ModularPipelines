using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "migration-jobs", "generate-ssh-script")]
public record GcloudDatabaseMigrationMigrationJobsGenerateSshScriptOptions(
[property: PositionalArgument] string MigrationJob,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--vm")] string Vm,
[property: CommandSwitch("--vm-zone")] string VmZone,
[property: CommandSwitch("--subnet")] string Subnet,
[property: CommandSwitch("--vm-machine-type")] string VmMachineType,
[property: CommandSwitch("--vm-zone-create")] string VmZoneCreate
) : GcloudOptions
{
    [CommandSwitch("--vm-port")]
    public string? VmPort { get; set; }
}