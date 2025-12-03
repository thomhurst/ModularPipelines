using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functions", "add-invoker-policy-binding")]
public record GcloudFunctionsAddInvokerPolicyBindingOptions(
[property: CliArgument] string Name,
[property: CliArgument] string Region,
[property: CliOption("--member")] string Member
) : GcloudOptions
{
    [CliFlag("--gen2")]
    public bool? Gen2 { get; set; }
}