using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "environments", "set-iam-policy")]
public record GcloudDataplexEnvironmentsSetIamPolicyOptions(
[property: PositionalArgument] string Environment,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;