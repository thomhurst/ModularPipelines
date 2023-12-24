using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("opsworkscm", "export-server-engine-attribute")]
public record AwsOpsworkscmExportServerEngineAttributeOptions(
[property: CommandSwitch("--export-attribute-name")] string ExportAttributeName,
[property: CommandSwitch("--server-name")] string ServerName
) : AwsOptions
{
    [CommandSwitch("--input-attributes")]
    public string[]? InputAttributes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}