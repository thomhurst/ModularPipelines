using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mysql", "flexible-server", "export", "create")]
public record AzMysqlFlexibleServerExportCreateOptions(
[property: CommandSwitch("--backup-name")] string BackupName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--sas-uri")] string SasUri
) : AzOptions
{
}

