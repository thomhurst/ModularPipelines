using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("directconnect", "confirm-customer-agreement")]
public record AwsDirectconnectConfirmCustomerAgreementOptions : AwsOptions
{
    [CliOption("--agreement-name")]
    public string? AgreementName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}