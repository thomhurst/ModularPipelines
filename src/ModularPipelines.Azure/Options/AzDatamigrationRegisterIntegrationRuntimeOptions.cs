using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datamigration", "register-integration-runtime")]
public record AzDatamigrationRegisterIntegrationRuntimeOptions(
[property: CommandSwitch("--auth-key")] string AuthKey
) : AzOptions
{
    [CommandSwitch("--installed-ir-path")]
    public string? InstalledIrPath { get; set; }

    [CommandSwitch("--ir-path")]
    public string? IrPath { get; set; }
}