using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "generate-login-token")]
public record GcloudSqlGenerateLoginTokenOptions : GcloudOptions
{
    [CliFlag("--application-default-credential")]
    public bool? ApplicationDefaultCredential { get; set; }

    [CliOption("--instance")]
    public string? Instance { get; set; }
}