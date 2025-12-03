using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mysql", "flexible-server", "export", "create")]
public record AzMysqlFlexibleServerExportCreateOptions(
[property: CliOption("--backup-name")] string BackupName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sas-uri")] string SasUri
) : AzOptions;