using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "assets", "set-iam-policy")]
public record GcloudDataplexAssetsSetIamPolicyOptions(
[property: PositionalArgument] string Asset,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Zone,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;