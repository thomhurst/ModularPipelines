using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifybackend", "update-backend-config")]
public record AwsAmplifybackendUpdateBackendConfigOptions(
[property: CliOption("--app-id")] string AppId
) : AwsOptions
{
    [CliOption("--login-auth-config")]
    public string? LoginAuthConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}