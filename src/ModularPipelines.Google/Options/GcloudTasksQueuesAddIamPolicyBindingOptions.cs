using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("tasks", "queues", "add-iam-policy-binding")]
public record GcloudTasksQueuesAddIamPolicyBindingOptions(
[property: PositionalArgument] string Queue,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--member")] string Member,
[property: CommandSwitch("--role")] string Role
) : GcloudOptions;