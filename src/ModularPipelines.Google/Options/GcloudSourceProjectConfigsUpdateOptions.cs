using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("source", "project-configs", "update")]
public record GcloudSourceProjectConfigsUpdateOptions(
[property: CliFlag("--disable-pushblock")] bool DisablePushblock,
[property: CliFlag("--enable-pushblock")] bool EnablePushblock,
[property: CliOption("--message-format")] string MessageFormat,
[property: CliOption("--service-account")] string ServiceAccount,
[property: CliOption("--topic-project")] string TopicProject,
[property: CliOption("--add-topic")] string AddTopic,
[property: CliOption("--remove-topic")] string RemoveTopic,
[property: CliOption("--update-topic")] string UpdateTopic
) : GcloudOptions;