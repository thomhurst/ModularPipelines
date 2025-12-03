using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("functions", "get-iam-policy")]
public record GcloudFunctionsGetIamPolicyOptions(
[property: CliArgument] string Name,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--gen2")]
    public bool? Gen2 { get; set; }
}