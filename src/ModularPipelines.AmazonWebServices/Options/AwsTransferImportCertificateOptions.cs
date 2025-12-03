using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "import-certificate")]
public record AwsTransferImportCertificateOptions(
[property: CliOption("--usage")] string Usage,
[property: CliOption("--certificate")] string Certificate
) : AwsOptions
{
    [CliOption("--certificate-chain")]
    public string? CertificateChain { get; set; }

    [CliOption("--private-key")]
    public string? PrivateKey { get; set; }

    [CliOption("--active-date")]
    public long? ActiveDate { get; set; }

    [CliOption("--inactive-date")]
    public long? InactiveDate { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}