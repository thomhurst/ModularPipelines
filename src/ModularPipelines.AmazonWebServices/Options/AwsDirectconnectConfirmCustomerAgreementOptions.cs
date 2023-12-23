using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("directconnect", "confirm-customer-agreement")]
public record AwsDirectconnectConfirmCustomerAgreementOptions : AwsOptions
{
    [CommandSwitch("--agreement-name")]
    public string? AgreementName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}