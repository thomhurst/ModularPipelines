using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "triggers", "create", "pubsub")]
public record GcloudBuildsTriggersCreatePubsubOptions(
[property: CommandSwitch("--trigger-config")] string TriggerConfig,
[property: CommandSwitch("--topic")] string Topic,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--region")] string Region,
[property: CommandSwitch("--[no-]require-approval")] string[] NoRequireApproval,
[property: CommandSwitch("--service-account")] string ServiceAccount,
[property: CommandSwitch("--subscription-filter")] string SubscriptionFilter,
[property: CommandSwitch("--substitutions")] IEnumerable<KeyValue> Substitutions,
[property: CommandSwitch("--build-config")] string BuildConfig,
[property: CommandSwitch("--inline-config")] string InlineConfig,
[property: CommandSwitch("--dockerfile")] string Dockerfile,
[property: CommandSwitch("--dockerfile-dir")] string DockerfileDir,
[property: CommandSwitch("--dockerfile-image")] string DockerfileImage,
[property: CommandSwitch("--branch")] string Branch,
[property: CommandSwitch("--tag")] string Tag,
[property: CommandSwitch("--repository")] string Repository,
[property: CommandSwitch("--repo")] string Repo,
[property: CommandSwitch("--repo-type")] string RepoType,
[property: CommandSwitch("--github-enterprise-config")] string GithubEnterpriseConfig
) : GcloudOptions;