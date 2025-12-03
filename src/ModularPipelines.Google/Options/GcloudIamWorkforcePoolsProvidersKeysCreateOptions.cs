using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workforce-pools", "providers", "keys", "create")]
public record GcloudIamWorkforcePoolsProvidersKeysCreateOptions(
[property: CliArgument] string Key,
[property: CliArgument] string Location,
[property: CliArgument] string Provider,
[property: CliArgument] string WorkforcePool,
[property: CliOption("--spec")] string Spec,
[property: CliOption("--use")] string Use
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}