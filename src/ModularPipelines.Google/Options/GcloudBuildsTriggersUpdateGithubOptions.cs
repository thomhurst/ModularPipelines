using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "triggers", "update", "github")]
public record GcloudBuildsTriggersUpdateGithubOptions(
[property: CliArgument] string Trigger,
[property: CliArgument] string Region,
[property: CliOption("--trigger-config")] string TriggerConfig,
[property: CliOption("--description")] string Description,
[property: CliOption("--ignored-files")] string[] IgnoredFiles,
[property: CliFlag("--include-logs-with-status")] bool IncludeLogsWithStatus,
[property: CliOption("--included-files")] string[] IncludedFiles,
[property: CliOption("--[no-]require-approval")] string[] NoRequireApproval,
[property: CliOption("--service-account")] string ServiceAccount,
[property: CliOption("--branch-pattern")] string BranchPattern,
[property: CliOption("--tag-pattern")] string TagPattern,
[property: CliOption("--comment-control")] string CommentControl,
[property: CliFlag("COMMENTS_DISABLED")] bool CommentsDisabled,
[property: CliFlag("COMMENTS_ENABLED")] bool CommentsEnabled,
[property: CliFlag("COMMENTS_ENABLED_FOR_EXTERNAL_CONTRIBUTORS_ONLY")] bool CommentsEnabledForExternalContributorsOnly,
[property: CliOption("--pull-request-pattern")] string PullRequestPattern,
[property: CliOption("--build-config")] string BuildConfig,
[property: CliOption("--inline-config")] string InlineConfig,
[property: CliOption("--dockerfile")] string Dockerfile,
[property: CliOption("--dockerfile-image")] string DockerfileImage,
[property: CliOption("--dockerfile-dir")] string DockerfileDir,
[property: CliFlag("--clear-substitutions")] bool ClearSubstitutions,
[property: CliOption("--remove-substitutions")] string[] RemoveSubstitutions,
[property: CliOption("--update-substitutions")] IEnumerable<KeyValue> UpdateSubstitutions,
[property: CliOption("--repository")] string Repository,
[property: CliOption("--enterprise-config")] string EnterpriseConfig,
[property: CliOption("--repo-name")] string RepoName,
[property: CliOption("--repo-owner")] string RepoOwner
) : GcloudOptions;