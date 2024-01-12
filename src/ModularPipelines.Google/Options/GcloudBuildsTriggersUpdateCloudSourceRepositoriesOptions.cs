using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "triggers", "update", "cloud-source-repositories")]
public record GcloudBuildsTriggersUpdateCloudSourceRepositoriesOptions(
[property: PositionalArgument] string Trigger,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--trigger-config")] string TriggerConfig,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--ignored-files")] string[] IgnoredFiles,
[property: CommandSwitch("--included-files")] string[] IncludedFiles,
[property: CommandSwitch("--repo")] string Repo,
[property: CommandSwitch("--[no-]require-approval")] string[] NoRequireApproval,
[property: CommandSwitch("--service-account")] string ServiceAccount,
[property: CommandSwitch("--branch-pattern")] string BranchPattern,
[property: CommandSwitch("--tag-pattern")] string TagPattern,
[property: CommandSwitch("--build-config")] string BuildConfig,
[property: CommandSwitch("--inline-config")] string InlineConfig,
[property: CommandSwitch("--dockerfile")] string Dockerfile,
[property: CommandSwitch("--dockerfile-image")] string DockerfileImage,
[property: CommandSwitch("--dockerfile-dir")] string DockerfileDir,
[property: BooleanCommandSwitch("--clear-substitutions")] bool ClearSubstitutions,
[property: CommandSwitch("--remove-substitutions")] string[] RemoveSubstitutions,
[property: CommandSwitch("--update-substitutions")] IEnumerable<KeyValue> UpdateSubstitutions
) : GcloudOptions;