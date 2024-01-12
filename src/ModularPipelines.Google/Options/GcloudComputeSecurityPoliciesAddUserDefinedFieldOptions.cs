using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "security-policies", "add-user-defined-field")]
public record GcloudComputeSecurityPoliciesAddUserDefinedFieldOptions(
[property: PositionalArgument] string Name,
[property: CommandSwitch("--base")] string Base,
[property: CommandSwitch("--offset")] string Offset,
[property: CommandSwitch("--size")] string Size,
[property: CommandSwitch("--user-defined-field-name")] string UserDefinedFieldName
) : GcloudOptions
{
    [CommandSwitch("--mask")]
    public string? Mask { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}