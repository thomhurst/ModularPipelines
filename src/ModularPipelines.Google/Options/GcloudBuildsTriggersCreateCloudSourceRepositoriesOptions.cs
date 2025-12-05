using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "triggers", "create", "cloud-source-repositories")]
public record GcloudBuildsTriggersCreateCloudSourceRepositoriesOptions(
[property: CliOption("--trigger-config")] string TriggerConfig,
[property: CliOption("--repo")] string Repo,
[property: CliOption("--description")] string Description,
[property: CliOption("--ignored-files")] string[] IgnoredFiles,
[property: CliOption("--included-files")] string[] IncludedFiles,
[property: CliOption("--name")] string Name,
[property: CliOption("--region")] string Region,
[property: CliOption("--[no-]require-approval")] string[] NoRequireApproval,
[property: CliOption("--service-account")] string ServiceAccount,
[property: CliOption("--substitutions")] IEnumerable<KeyValue> Substitutions,
[property: CliOption("--branch-pattern")] string BranchPattern,
[property: CliOption("--tag-pattern")] string TagPattern,
[property: CliOption("--build-config")] string BuildConfig,
[property: CliOption("--inline-config")] string InlineConfig,
[property: CliOption("--dockerfile")] string Dockerfile,
[property: CliOption("--dockerfile-dir")] string DockerfileDir,
[property: CliOption("--dockerfile-image")] string DockerfileImage
) : GcloudOptions;