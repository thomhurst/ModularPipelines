using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "environments", "set-iam-policy")]
public record GcloudDataplexEnvironmentsSetIamPolicyOptions(
[property: CliArgument] string Environment,
[property: CliArgument] string Lake,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;