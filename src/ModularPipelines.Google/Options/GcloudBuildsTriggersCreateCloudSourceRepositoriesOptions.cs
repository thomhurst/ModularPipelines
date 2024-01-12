using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "triggers", "create", "cloud-source-repositories")]
public record GcloudBuildsTriggersCreateCloudSourceRepositoriesOptions(
[property: CommandSwitch("--trigger-config")] string TriggerConfig,
[property: CommandSwitch("--repo")] string Repo,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--ignored-files")] string[] IgnoredFiles,
[property: CommandSwitch("--included-files")] string[] IncludedFiles,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--region")] string Region,
[property: CommandSwitch("--[no-]require-approval")] string[] NoRequireApproval,
[property: CommandSwitch("--service-account")] string ServiceAccount,
[property: CommandSwitch("--substitutions")] IEnumerable<KeyValue> Substitutions,
[property: CommandSwitch("--branch-pattern")] string BranchPattern,
[property: CommandSwitch("--tag-pattern")] string TagPattern,
[property: CommandSwitch("--build-config")] string BuildConfig,
[property: CommandSwitch("--inline-config")] string InlineConfig,
[property: CommandSwitch("--dockerfile")] string Dockerfile,
[property: CommandSwitch("--dockerfile-dir")] string DockerfileDir,
[property: CommandSwitch("--dockerfile-image")] string DockerfileImage
) : GcloudOptions;