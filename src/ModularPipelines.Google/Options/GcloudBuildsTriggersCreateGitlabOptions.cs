using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "triggers", "create", "gitlab")]
public record GcloudBuildsTriggersCreateGitlabOptions(
[property: CommandSwitch("--trigger-config")] string TriggerConfig,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--ignored-files")] string[] IgnoredFiles,
[property: CommandSwitch("--included-files")] string[] IncludedFiles,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--region")] string Region,
[property: CommandSwitch("--repository")] string Repository,
[property: CommandSwitch("--[no-]require-approval")] string[] NoRequireApproval,
[property: CommandSwitch("--service-account")] string ServiceAccount,
[property: CommandSwitch("--substitutions")] IEnumerable<KeyValue> Substitutions,
[property: CommandSwitch("--branch-pattern")] string BranchPattern,
[property: CommandSwitch("--tag-pattern")] string TagPattern,
[property: CommandSwitch("--pull-request-pattern")] string PullRequestPattern,
[property: CommandSwitch("--comment-control")] string CommentControl,
[property: BooleanCommandSwitch("COMMENTS_DISABLED")] bool CommentsDisabled,
[property: BooleanCommandSwitch("COMMENTS_ENABLED")] bool CommentsEnabled,
[property: BooleanCommandSwitch("COMMENTS_ENABLED_FOR_EXTERNAL_CONTRIBUTORS_ONLY")] bool CommentsEnabledForExternalContributorsOnly,
[property: CommandSwitch("--build-config")] string BuildConfig,
[property: CommandSwitch("--inline-config")] string InlineConfig,
[property: CommandSwitch("--dockerfile")] string Dockerfile,
[property: CommandSwitch("--dockerfile-image")] string DockerfileImage,
[property: CommandSwitch("--dockerfile-dir")] string DockerfileDir
) : GcloudOptions;