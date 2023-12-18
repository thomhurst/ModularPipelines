using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

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

