using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workload-identity-pools", "create-cred-config")]
public record GcloudIamWorkloadIdentityPoolsCreateCredConfigOptions(
[property: PositionalArgument] string Audience,
[property: CommandSwitch("--output-file")] string OutputFile,
[property: BooleanCommandSwitch("--aws")] bool Aws,
[property: BooleanCommandSwitch("--azure")] bool Azure,
[property: CommandSwitch("--credential-source-file")] string CredentialSourceFile,
[property: CommandSwitch("--credential-source-url")] string CredentialSourceUrl,
[property: CommandSwitch("--executable-command")] string ExecutableCommand
) : GcloudOptions
{
    [CommandSwitch("--app-id-uri")]
    public string? AppIdUri { get; set; }

    [CommandSwitch("--credential-source-field-name")]
    public string? CredentialSourceFieldName { get; set; }

    [CommandSwitch("--credential-source-headers")]
    public string[]? CredentialSourceHeaders { get; set; }

    [CommandSwitch("--credential-source-type")]
    public string? CredentialSourceType { get; set; }

    [BooleanCommandSwitch("--enable-imdsv2")]
    public bool? EnableImdsv2 { get; set; }

    [CommandSwitch("--subject-token-type")]
    public string? SubjectTokenType { get; set; }

    [CommandSwitch("--executable-output-file")]
    public string? ExecutableOutputFile { get; set; }

    [CommandSwitch("--executable-timeout-millis")]
    public string? ExecutableTimeoutMillis { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--service-account-token-lifetime-seconds")]
    public string? ServiceAccountTokenLifetimeSeconds { get; set; }
}