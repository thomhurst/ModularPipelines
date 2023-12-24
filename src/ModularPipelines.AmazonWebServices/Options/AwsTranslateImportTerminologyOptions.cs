using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("translate", "import-terminology")]
public record AwsTranslateImportTerminologyOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--merge-strategy")] string MergeStrategy,
[property: CommandSwitch("--data-file")] string DataFile
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--terminology-data")]
    public string? TerminologyData { get; set; }

    [CommandSwitch("--encryption-key")]
    public string? EncryptionKey { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}