using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "databases", "create")]
public record GcloudSpannerDatabasesCreateOptions(
[property: PositionalArgument] string Database,
[property: PositionalArgument] string Instance
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--database-dialect")]
    public string? DatabaseDialect { get; set; }

    [CommandSwitch("--ddl")]
    public string? Ddl { get; set; }

    [CommandSwitch("--ddl-file")]
    public string? DdlFile { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--kms-keyring")]
    public string? KmsKeyring { get; set; }

    [CommandSwitch("--kms-location")]
    public string? KmsLocation { get; set; }

    [CommandSwitch("--kms-project")]
    public string? KmsProject { get; set; }
}