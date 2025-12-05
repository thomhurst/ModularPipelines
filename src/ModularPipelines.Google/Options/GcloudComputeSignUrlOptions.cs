using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "sign-url")]
public record GcloudComputeSignUrlOptions(
[property: CliArgument] string Url,
[property: CliOption("--expires-in")] string ExpiresIn,
[property: CliOption("--key-file")] string KeyFile,
[property: CliOption("--key-name")] string KeyName
) : GcloudOptions
{
    [CliFlag("--validate")]
    public bool? Validate { get; set; }
}