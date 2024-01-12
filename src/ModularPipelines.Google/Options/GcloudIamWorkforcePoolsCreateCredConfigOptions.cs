using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iam", "workforce-pools", "create-cred-config")]
public record GcloudIamWorkforcePoolsCreateCredConfigOptions(
[property: PositionalArgument] string Audience,
[property: CommandSwitch("--output-file")] string OutputFile,
[property: CommandSwitch("--workforce-pool-user-project")] string WorkforcePoolUserProject,
[property: CommandSwitch("--credential-source-file")] string CredentialSourceFile,
[property: CommandSwitch("--credential-source-url")] string CredentialSourceUrl,
[property: CommandSwitch("--executable-command")] string ExecutableCommand
) : GcloudOptions
{
    [CommandSwitch("--credential-source-field-name")]
    public string? CredentialSourceFieldName { get; set; }

    [CommandSwitch("--credential-source-headers")]
    public string[]? CredentialSourceHeaders { get; set; }

    [CommandSwitch("--credential-source-type")]
    public string? CredentialSourceType { get; set; }

    [CommandSwitch("--subject-token-type")]
    public string? SubjectTokenType { get; set; }

    [CommandSwitch("--executable-interactive-timeout-millis")]
    public string? ExecutableInteractiveTimeoutMillis { get; set; }

    [CommandSwitch("--executable-output-file")]
    public string? ExecutableOutputFile { get; set; }

    [CommandSwitch("--executable-timeout-millis")]
    public string? ExecutableTimeoutMillis { get; set; }

    [CommandSwitch("--service-account")]
    public string? ServiceAccount { get; set; }

    [CommandSwitch("--service-account-token-lifetime-seconds")]
    public string? ServiceAccountTokenLifetimeSeconds { get; set; }
}