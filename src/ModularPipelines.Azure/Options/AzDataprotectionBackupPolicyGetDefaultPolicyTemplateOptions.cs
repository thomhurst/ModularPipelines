using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataprotection", "backup-policy", "get-default-policy-template")]
public record AzDataprotectionBackupPolicyGetDefaultPolicyTemplateOptions(
[property: CliOption("--datasource-type")] string DatasourceType
) : AzOptions;