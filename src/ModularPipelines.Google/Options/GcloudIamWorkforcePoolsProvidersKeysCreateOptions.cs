using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workforce-pools", "providers", "keys", "create")]
public record GcloudIamWorkforcePoolsProvidersKeysCreateOptions(
[property: PositionalArgument] string Key,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Provider,
[property: PositionalArgument] string WorkforcePool,
[property: CommandSwitch("--spec")] string Spec,
[property: CommandSwitch("--use")] string Use
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}