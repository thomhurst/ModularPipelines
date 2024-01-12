using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "generate-login-token")]
public record GcloudSqlGenerateLoginTokenOptions : GcloudOptions
{
    [BooleanCommandSwitch("--application-default-credential")]
    public bool? ApplicationDefaultCredential { get; set; }

    [CommandSwitch("--instance")]
    public string? Instance { get; set; }
}