using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "triggers", "update", "bitbucketserver")]
public record GcloudBuildsTriggersUpdateBitbucketserverOptions(
[property: PositionalArgument] string Trigger,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--trigger-config")] string TriggerConfig,
[property: CommandSwitch("--bitbucket-server-config-resource")] string BitbucketServerConfigResource,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--ignored-files")] string[] IgnoredFiles,
[property: CommandSwitch("--included-files")] string[] IncludedFiles,
[property: CommandSwitch("--project-key")] string ProjectKey,
[property: CommandSwitch("--repo-slug")] string RepoSlug,
[property: CommandSwitch("--[no-]require-approval")] string[] NoRequireApproval,
[property: CommandSwitch("--service-account")] string ServiceAccount,
[property: CommandSwitch("--branch-pattern")] string BranchPattern,
[property: CommandSwitch("--tag-pattern")] string TagPattern,
[property: CommandSwitch("--comment-control")] string CommentControl,
[property: BooleanCommandSwitch("COMMENTS_DISABLED")] bool CommentsDisabled,
[property: BooleanCommandSwitch("COMMENTS_ENABLED")] bool CommentsEnabled,
[property: BooleanCommandSwitch("COMMENTS_ENABLED_FOR_EXTERNAL_CONTRIBUTORS_ONLY")] bool CommentsEnabledForExternalContributorsOnly,
[property: CommandSwitch("--pull-request-pattern")] string PullRequestPattern,
[property: CommandSwitch("--build-config")] string BuildConfig,
[property: CommandSwitch("--inline-config")] string InlineConfig,
[property: CommandSwitch("--dockerfile")] string Dockerfile,
[property: CommandSwitch("--dockerfile-image")] string DockerfileImage,
[property: CommandSwitch("--dockerfile-dir")] string DockerfileDir,
[property: BooleanCommandSwitch("--clear-substitutions")] bool ClearSubstitutions,
[property: CommandSwitch("--remove-substitutions")] string[] RemoveSubstitutions,
[property: CommandSwitch("--update-substitutions")] IEnumerable<KeyValue> UpdateSubstitutions
) : GcloudOptions;