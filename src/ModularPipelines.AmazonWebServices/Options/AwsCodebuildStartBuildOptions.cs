using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codebuild", "start-build")]
public record AwsCodebuildStartBuildOptions(
[property: CommandSwitch("--project-name")] string ProjectName
) : AwsOptions
{
    [CommandSwitch("--secondary-sources-override")]
    public string[]? SecondarySourcesOverride { get; set; }

    [CommandSwitch("--secondary-sources-version-override")]
    public string[]? SecondarySourcesVersionOverride { get; set; }

    [CommandSwitch("--source-version")]
    public string? SourceVersion { get; set; }

    [CommandSwitch("--artifacts-override")]
    public string? ArtifactsOverride { get; set; }

    [CommandSwitch("--secondary-artifacts-override")]
    public string[]? SecondaryArtifactsOverride { get; set; }

    [CommandSwitch("--environment-variables-override")]
    public string[]? EnvironmentVariablesOverride { get; set; }

    [CommandSwitch("--source-type-override")]
    public string? SourceTypeOverride { get; set; }

    [CommandSwitch("--source-location-override")]
    public string? SourceLocationOverride { get; set; }

    [CommandSwitch("--source-auth-override")]
    public string? SourceAuthOverride { get; set; }

    [CommandSwitch("--git-clone-depth-override")]
    public int? GitCloneDepthOverride { get; set; }

    [CommandSwitch("--git-submodules-config-override")]
    public string? GitSubmodulesConfigOverride { get; set; }

    [CommandSwitch("--buildspec-override")]
    public string? BuildspecOverride { get; set; }

    [CommandSwitch("--build-status-config-override")]
    public string? BuildStatusConfigOverride { get; set; }

    [CommandSwitch("--environment-type-override")]
    public string? EnvironmentTypeOverride { get; set; }

    [CommandSwitch("--image-override")]
    public string? ImageOverride { get; set; }

    [CommandSwitch("--compute-type-override")]
    public string? ComputeTypeOverride { get; set; }

    [CommandSwitch("--certificate-override")]
    public string? CertificateOverride { get; set; }

    [CommandSwitch("--cache-override")]
    public string? CacheOverride { get; set; }

    [CommandSwitch("--service-role-override")]
    public string? ServiceRoleOverride { get; set; }

    [CommandSwitch("--timeout-in-minutes-override")]
    public int? TimeoutInMinutesOverride { get; set; }

    [CommandSwitch("--queued-timeout-in-minutes-override")]
    public int? QueuedTimeoutInMinutesOverride { get; set; }

    [CommandSwitch("--encryption-key-override")]
    public string? EncryptionKeyOverride { get; set; }

    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--logs-config-override")]
    public string? LogsConfigOverride { get; set; }

    [CommandSwitch("--registry-credential-override")]
    public string? RegistryCredentialOverride { get; set; }

    [CommandSwitch("--image-pull-credentials-type-override")]
    public string? ImagePullCredentialsTypeOverride { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}