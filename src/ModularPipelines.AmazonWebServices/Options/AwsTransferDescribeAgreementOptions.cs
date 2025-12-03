using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("transfer", "describe-agreement")]
public record AwsTransferDescribeAgreementOptions(
[property: CliOption("--agreement-id")] string AgreementId,
[property: CliOption("--server-id")] string ServerId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}