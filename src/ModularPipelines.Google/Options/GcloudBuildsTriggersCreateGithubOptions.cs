using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "triggers", "create", "github")]
public record GcloudBuildsTriggersCreateGithubOptions(
[property: CliOption("--trigger-config")] string TriggerConfig,
[property: CliOption("--description")] string Description,
[property: CliOption("--ignored-files")] string[] IgnoredFiles,
[property: CliFlag("--include-logs-with-status")] bool IncludeLogsWithStatus,
[property: CliOption("--included-files")] string[] IncludedFiles,
[property: CliOption("--name")] string Name,
[property: CliOption("--region")] string Region,
[property: CliOption("--[no-]require-approval")] string[] NoRequireApproval,
[property: CliOption("--service-account")] string ServiceAccount,
[property: CliOption("--substitutions")] IEnumerable<KeyValue> Substitutions,
[property: CliOption("--branch-pattern")] string BranchPattern,
[property: CliOption("--tag-pattern")] string TagPattern,
[property: CliOption("--pull-request-pattern")] string PullRequestPattern,
[property: CliOption("--comment-control")] string CommentControl,
[property: CliFlag("COMMENTS_DISABLED")] bool CommentsDisabled,
[property: CliFlag("COMMENTS_ENABLED")] bool CommentsEnabled,
[property: CliFlag("COMMENTS_ENABLED_FOR_EXTERNAL_CONTRIBUTORS_ONLY")] bool CommentsEnabledForExternalContributorsOnly,
[property: CliOption("--build-config")] string BuildConfig,
[property: CliOption("--inline-config")] string InlineConfig,
[property: CliOption("--dockerfile")] string Dockerfile,
[property: CliOption("--dockerfile-dir")] string DockerfileDir,
[property: CliOption("--dockerfile-image")] string DockerfileImage,
[property: CliOption("--repository")] string Repository,
[property: CliOption("--repo-name")] string RepoName,
[property: CliOption("--repo-owner")] string RepoOwner,
[property: CliOption("--enterprise-config")] string EnterpriseConfig
) : GcloudOptions;