using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "triggers", "create", "webhook")]
public record GcloudBuildsTriggersCreateWebhookOptions(
[property: CliOption("--trigger-config")] string TriggerConfig,
[property: CliOption("--secret")] string Secret,
[property: CliOption("--description")] string Description,
[property: CliOption("--name")] string Name,
[property: CliOption("--region")] string Region,
[property: CliOption("--[no-]require-approval")] string[] NoRequireApproval,
[property: CliOption("--service-account")] string ServiceAccount,
[property: CliOption("--subscription-filter")] string SubscriptionFilter,
[property: CliOption("--substitutions")] IEnumerable<KeyValue> Substitutions,
[property: CliOption("--build-config")] string BuildConfig,
[property: CliOption("--inline-config")] string InlineConfig,
[property: CliOption("--dockerfile")] string Dockerfile,
[property: CliOption("--dockerfile-dir")] string DockerfileDir,
[property: CliOption("--dockerfile-image")] string DockerfileImage,
[property: CliOption("--branch")] string Branch,
[property: CliOption("--tag")] string Tag,
[property: CliOption("--repository")] string Repository,
[property: CliOption("--repo")] string Repo,
[property: CliOption("--repo-type")] string RepoType,
[property: CliOption("--github-enterprise-config")] string GithubEnterpriseConfig
) : GcloudOptions;