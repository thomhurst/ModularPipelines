using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dataplex", "tasks", "set-iam-policy")]
public record GcloudDataplexTasksSetIamPolicyOptions(
[property: CliArgument] string Task,
[property: CliArgument] string Lake,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;