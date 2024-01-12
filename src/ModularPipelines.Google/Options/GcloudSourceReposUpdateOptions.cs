using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("source", "repos", "update")]
public record GcloudSourceReposUpdateOptions(
[property: PositionalArgument] string Repo,
[property: CommandSwitch("--message-format")] string MessageFormat,
[property: CommandSwitch("--service-account")] string ServiceAccount,
[property: CommandSwitch("--topic-project")] string TopicProject,
[property: CommandSwitch("--add-topic")] string AddTopic,
[property: CommandSwitch("--remove-topic")] string RemoveTopic,
[property: CommandSwitch("--update-topic")] string UpdateTopic
) : GcloudOptions;