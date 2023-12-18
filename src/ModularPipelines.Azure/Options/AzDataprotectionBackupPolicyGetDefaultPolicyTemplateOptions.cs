using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-policy", "get-default-policy-template")]
public record AzDataprotectionBackupPolicyGetDefaultPolicyTemplateOptions(
[property: CommandSwitch("--datasource-type")] string DatasourceType
) : AzOptions
{
}