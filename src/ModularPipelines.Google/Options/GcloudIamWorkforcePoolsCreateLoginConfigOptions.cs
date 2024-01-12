using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workforce-pools", "create-login-config")]
public record GcloudIamWorkforcePoolsCreateLoginConfigOptions(
[property: PositionalArgument] string Audience,
[property: CommandSwitch("--output-file")] string OutputFile
) : GcloudOptions
{
    [BooleanCommandSwitch("--activate")]
    public bool? Activate { get; set; }
}