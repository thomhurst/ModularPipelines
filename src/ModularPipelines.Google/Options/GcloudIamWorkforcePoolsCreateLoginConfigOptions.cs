using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "create-login-config")]
public record GcloudIamWorkforcePoolsCreateLoginConfigOptions(
[property: CliArgument] string Audience,
[property: CliOption("--output-file")] string OutputFile
) : GcloudOptions
{
    [CliFlag("--activate")]
    public bool? Activate { get; set; }
}