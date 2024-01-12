using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "security-policies", "remove-user-defined-field")]
public record GcloudComputeSecurityPoliciesRemoveUserDefinedFieldOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--user-defined-field-name")] string UserDefinedFieldName
) : GcloudOptions
{
    [CommandSwitch("--region")]
    public string? Region { get; set; }
}