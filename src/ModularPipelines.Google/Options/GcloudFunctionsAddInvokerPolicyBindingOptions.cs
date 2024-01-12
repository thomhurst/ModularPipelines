using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functions", "add-invoker-policy-binding")]
public record GcloudFunctionsAddInvokerPolicyBindingOptions(
[property: PositionalArgument] string Name,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--member")] string Member
) : GcloudOptions
{
    [BooleanCommandSwitch("--gen2")]
    public bool? Gen2 { get; set; }
}