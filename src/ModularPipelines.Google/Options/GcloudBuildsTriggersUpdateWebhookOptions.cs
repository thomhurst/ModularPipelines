using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "triggers", "update", "webhook")]
public record GcloudBuildsTriggersUpdateWebhookOptions(
[property: CliArgument] string Trigger,
[property: CliArgument] string Region,
[property: CliOption("--trigger-config")] string TriggerConfig,
[property: CliOption("--description")] string Description,
[property: CliOption("--[no-]require-approval")] string[] NoRequireApproval,
[property: CliOption("--secret")] string Secret,
[property: CliOption("--service-account")] string ServiceAccount,
[property: CliOption("--subscription-filter")] string SubscriptionFilter,
[property: CliFlag("--clear-substitutions")] bool ClearSubstitutions,
[property: CliOption("--remove-substitutions")] string[] RemoveSubstitutions,
[property: CliOption("--update-substitutions")] IEnumerable<KeyValue> UpdateSubstitutions,
[property: CliOption("--inline-config")] string InlineConfig,
[property: CliOption("--dockerfile")] string Dockerfile,
[property: CliOption("--dockerfile-dir")] string DockerfileDir,
[property: CliOption("--dockerfile-image")] string DockerfileImage,
[property: CliOption("--git-file-source-branch")] string GitFileSourceBranch,
[property: CliOption("--git-file-source-tag")] string GitFileSourceTag,
[property: CliOption("--git-file-source-github-enterprise-config")] string GitFileSourceGithubEnterpriseConfig,
[property: CliOption("--git-file-source-path")] string GitFileSourcePath,
[property: CliOption("--git-file-source-repo-type")] string GitFileSourceRepoType,
[property: CliOption("--git-file-source-uri")] string GitFileSourceUri,
[property: CliOption("--source-to-build-branch")] string SourceToBuildBranch,
[property: CliOption("--source-to-build-tag")] string SourceToBuildTag,
[property: CliOption("--source-to-build-github-enterprise-config")] string SourceToBuildGithubEnterpriseConfig,
[property: CliOption("--source-to-build-repo-type")] string SourceToBuildRepoType,
[property: CliOption("--source-to-build-uri")] string SourceToBuildUri
) : GcloudOptions;