using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functions", "remove-iam-policy-binding")]
public record GcloudFunctionsRemoveIamPolicyBindingOptions(
[property: PositionalArgument] string Name,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions
{
    [BooleanCommandSwitch("--gen2")]
    public bool? Gen2 { get; set; }
}