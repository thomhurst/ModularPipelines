using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "content", "set-iam-policy")]
public record GcloudDataplexContentSetIamPolicyOptions(
[property: CliArgument] string Content,
[property: CliArgument] string Lake,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;