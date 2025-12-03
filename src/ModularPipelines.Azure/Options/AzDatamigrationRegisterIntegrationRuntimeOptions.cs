using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datamigration", "register-integration-runtime")]
public record AzDatamigrationRegisterIntegrationRuntimeOptions(
[property: CliOption("--auth-key")] string AuthKey
) : AzOptions
{
    [CliOption("--installed-ir-path")]
    public string? InstalledIrPath { get; set; }

    [CliOption("--ir-path")]
    public string? IrPath { get; set; }
}