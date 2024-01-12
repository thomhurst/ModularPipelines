using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dataplex", "content", "set-iam-policy")]
public record GcloudDataplexContentSetIamPolicyOptions(
[property: PositionalArgument] string Content,
[property: PositionalArgument] string Lake,
[property: PositionalArgument] string Location,
[property: PositionalArgument] string PolicyFile
) : GcloudOptions;