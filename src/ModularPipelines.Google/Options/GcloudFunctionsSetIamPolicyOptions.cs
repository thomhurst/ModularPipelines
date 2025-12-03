using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functions", "set-iam-policy")]
public record GcloudFunctionsSetIamPolicyOptions(
[property: CliArgument] string Name,
[property: CliArgument] string Region,
[property: CliArgument] string PolicyFile
) : GcloudOptions
{
    [CliFlag("--gen2")]
    public bool? Gen2 { get; set; }
}