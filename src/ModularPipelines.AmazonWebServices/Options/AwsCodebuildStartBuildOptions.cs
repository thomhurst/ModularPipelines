using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codebuild", "start-build")]
public record AwsCodebuildStartBuildOptions(
[property: CliOption("--project-name")] string ProjectName
) : AwsOptions
{
    [CliOption("--secondary-sources-override")]
    public string[]? SecondarySourcesOverride { get; set; }

    [CliOption("--secondary-sources-version-override")]
    public string[]? SecondarySourcesVersionOverride { get; set; }

    [CliOption("--source-version")]
    public string? SourceVersion { get; set; }

    [CliOption("--artifacts-override")]
    public string? ArtifactsOverride { get; set; }

    [CliOption("--secondary-artifacts-override")]
    public string[]? SecondaryArtifactsOverride { get; set; }

    [CliOption("--environment-variables-override")]
    public string[]? EnvironmentVariablesOverride { get; set; }

    [CliOption("--source-type-override")]
    public string? SourceTypeOverride { get; set; }

    [CliOption("--source-location-override")]
    public string? SourceLocationOverride { get; set; }

    [CliOption("--source-auth-override")]
    public string? SourceAuthOverride { get; set; }

    [CliOption("--git-clone-depth-override")]
    public int? GitCloneDepthOverride { get; set; }

    [CliOption("--git-submodules-config-override")]
    public string? GitSubmodulesConfigOverride { get; set; }

    [CliOption("--buildspec-override")]
    public string? BuildspecOverride { get; set; }

    [CliOption("--build-status-config-override")]
    public string? BuildStatusConfigOverride { get; set; }

    [CliOption("--environment-type-override")]
    public string? EnvironmentTypeOverride { get; set; }

    [CliOption("--image-override")]
    public string? ImageOverride { get; set; }

    [CliOption("--compute-type-override")]
    public string? ComputeTypeOverride { get; set; }

    [CliOption("--certificate-override")]
    public string? CertificateOverride { get; set; }

    [CliOption("--cache-override")]
    public string? CacheOverride { get; set; }

    [CliOption("--service-role-override")]
    public string? ServiceRoleOverride { get; set; }

    [CliOption("--timeout-in-minutes-override")]
    public int? TimeoutInMinutesOverride { get; set; }

    [CliOption("--queued-timeout-in-minutes-override")]
    public int? QueuedTimeoutInMinutesOverride { get; set; }

    [CliOption("--encryption-key-override")]
    public string? EncryptionKeyOverride { get; set; }

    [CliOption("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CliOption("--logs-config-override")]
    public string? LogsConfigOverride { get; set; }

    [CliOption("--registry-credential-override")]
    public string? RegistryCredentialOverride { get; set; }

    [CliOption("--image-pull-credentials-type-override")]
    public string? ImagePullCredentialsTypeOverride { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}