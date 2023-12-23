using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("transfer", "import-certificate")]
public record AwsTransferImportCertificateOptions(
[property: CommandSwitch("--usage")] string Usage,
[property: CommandSwitch("--certificate")] string Certificate
) : AwsOptions
{
    [CommandSwitch("--certificate-chain")]
    public string? CertificateChain { get; set; }

    [CommandSwitch("--private-key")]
    public string? PrivateKey { get; set; }

    [CommandSwitch("--active-date")]
    public long? ActiveDate { get; set; }

    [CommandSwitch("--inactive-date")]
    public long? InactiveDate { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}