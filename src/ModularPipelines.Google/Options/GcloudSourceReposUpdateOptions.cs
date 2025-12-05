using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("source", "repos", "update")]
public record GcloudSourceReposUpdateOptions(
[property: CliArgument] string Repo,
[property: CliOption("--message-format")] string MessageFormat,
[property: CliOption("--service-account")] string ServiceAccount,
[property: CliOption("--topic-project")] string TopicProject,
[property: CliOption("--add-topic")] string AddTopic,
[property: CliOption("--remove-topic")] string RemoveTopic,
[property: CliOption("--update-topic")] string UpdateTopic
) : GcloudOptions;