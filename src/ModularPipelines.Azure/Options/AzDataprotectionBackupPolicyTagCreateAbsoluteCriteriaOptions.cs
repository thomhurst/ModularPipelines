using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataprotection", "backup-policy", "tag", "create-absolute-criteria")]
public record AzDataprotectionBackupPolicyTagCreateAbsoluteCriteriaOptions(
[property: CommandSwitch("--absolute-criteria")] string AbsoluteCriteria
) : AzOptions
{
}