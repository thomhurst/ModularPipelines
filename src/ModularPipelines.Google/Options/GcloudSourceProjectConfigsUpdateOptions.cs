using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("source", "project-configs", "update")]
public record GcloudSourceProjectConfigsUpdateOptions(
[property: BooleanCommandSwitch("--disable-pushblock")] bool DisablePushblock,
[property: BooleanCommandSwitch("--enable-pushblock")] bool EnablePushblock,
[property: CommandSwitch("--message-format")] string MessageFormat,
[property: CommandSwitch("--service-account")] string ServiceAccount,
[property: CommandSwitch("--topic-project")] string TopicProject,
[property: CommandSwitch("--add-topic")] string AddTopic,
[property: CommandSwitch("--remove-topic")] string RemoveTopic,
[property: CommandSwitch("--update-topic")] string UpdateTopic
) : GcloudOptions;