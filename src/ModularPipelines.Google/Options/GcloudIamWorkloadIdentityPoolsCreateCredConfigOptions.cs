using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iam", "workload-identity-pools", "create-cred-config")]
public record GcloudIamWorkloadIdentityPoolsCreateCredConfigOptions(
[property: CliArgument] string Audience,
[property: CliOption("--output-file")] string OutputFile,
[property: CliFlag("--aws")] bool Aws,
[property: CliFlag("--azure")] bool Azure,
[property: CliOption("--credential-source-file")] string CredentialSourceFile,
[property: CliOption("--credential-source-url")] string CredentialSourceUrl,
[property: CliOption("--executable-command")] string ExecutableCommand
) : GcloudOptions
{
    [CliOption("--app-id-uri")]
    public string? AppIdUri { get; set; }

    [CliOption("--credential-source-field-name")]
    public string? CredentialSourceFieldName { get; set; }

    [CliOption("--credential-source-headers")]
    public string[]? CredentialSourceHeaders { get; set; }

    [CliOption("--credential-source-type")]
    public string? CredentialSourceType { get; set; }

    [CliFlag("--enable-imdsv2")]
    public bool? EnableImdsv2 { get; set; }

    [CliOption("--subject-token-type")]
    public string? SubjectTokenType { get; set; }

    [CliOption("--executable-output-file")]
    public string? ExecutableOutputFile { get; set; }

    [CliOption("--executable-timeout-millis")]
    public string? ExecutableTimeoutMillis { get; set; }

    [CliOption("--service-account")]
    public string? ServiceAccount { get; set; }

    [CliOption("--service-account-token-lifetime-seconds")]
    public string? ServiceAccountTokenLifetimeSeconds { get; set; }
}