using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("tasks", "queues", "set-iam-policy")]
public record GcloudTasksQueuesSetIamPolicyOptions(
[property: CliArgument] string Queue,
[property: CliArgument] string Location,
[property: CliArgument] string PolicyFile
) : GcloudOptions;