using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "assets", "get-iam-policy")]
public record GcloudDataplexAssetsGetIamPolicyOptions(
[property: PositionalArgument] string Asset,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string Zone
) : GcloudOptions;