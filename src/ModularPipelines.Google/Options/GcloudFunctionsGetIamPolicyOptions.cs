using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functions", "get-iam-policy")]
public record GcloudFunctionsGetIamPolicyOptions(
[property: PositionalArgument] string Name,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--gen2")]
    public bool? Gen2 { get; set; }
}