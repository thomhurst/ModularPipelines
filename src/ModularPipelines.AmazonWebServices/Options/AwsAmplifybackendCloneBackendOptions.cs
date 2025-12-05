using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifybackend", "clone-backend")]
public record AwsAmplifybackendCloneBackendOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--backend-environment-name")] string BackendEnvironmentName,
[property: CliOption("--target-environment-name")] string TargetEnvironmentName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}