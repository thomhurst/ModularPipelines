using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("payment-cryptography", "export-key")]
public record AwsPaymentCryptographyExportKeyOptions(
[property: CommandSwitch("--export-key-identifier")] string ExportKeyIdentifier,
[property: CommandSwitch("--key-material")] string KeyMaterial
) : AwsOptions
{
    [CommandSwitch("--export-attributes")]
    public string? ExportAttributes { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}