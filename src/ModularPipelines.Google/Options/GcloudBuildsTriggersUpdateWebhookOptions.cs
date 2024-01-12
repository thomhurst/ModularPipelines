using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "triggers", "update", "webhook")]
public record GcloudBuildsTriggersUpdateWebhookOptions(
[property: PositionalArgument] string Trigger,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--trigger-config")] string TriggerConfig,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--[no-]require-approval")] string[] NoRequireApproval,
[property: CommandSwitch("--secret")] string Secret,
[property: CommandSwitch("--service-account")] string ServiceAccount,
[property: CommandSwitch("--subscription-filter")] string SubscriptionFilter,
[property: BooleanCommandSwitch("--clear-substitutions")] bool ClearSubstitutions,
[property: CommandSwitch("--remove-substitutions")] string[] RemoveSubstitutions,
[property: CommandSwitch("--update-substitutions")] IEnumerable<KeyValue> UpdateSubstitutions,
[property: CommandSwitch("--inline-config")] string InlineConfig,
[property: CommandSwitch("--dockerfile")] string Dockerfile,
[property: CommandSwitch("--dockerfile-dir")] string DockerfileDir,
[property: CommandSwitch("--dockerfile-image")] string DockerfileImage,
[property: CommandSwitch("--git-file-source-branch")] string GitFileSourceBranch,
[property: CommandSwitch("--git-file-source-tag")] string GitFileSourceTag,
[property: CommandSwitch("--git-file-source-github-enterprise-config")] string GitFileSourceGithubEnterpriseConfig,
[property: CommandSwitch("--git-file-source-path")] string GitFileSourcePath,
[property: CommandSwitch("--git-file-source-repo-type")] string GitFileSourceRepoType,
[property: CommandSwitch("--git-file-source-uri")] string GitFileSourceUri,
[property: CommandSwitch("--source-to-build-branch")] string SourceToBuildBranch,
[property: CommandSwitch("--source-to-build-tag")] string SourceToBuildTag,
[property: CommandSwitch("--source-to-build-github-enterprise-config")] string SourceToBuildGithubEnterpriseConfig,
[property: CommandSwitch("--source-to-build-repo-type")] string SourceToBuildRepoType,
[property: CommandSwitch("--source-to-build-uri")] string SourceToBuildUri
) : GcloudOptions;