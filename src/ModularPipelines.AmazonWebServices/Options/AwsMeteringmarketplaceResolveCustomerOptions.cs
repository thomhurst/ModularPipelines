using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("meteringmarketplace", "resolve-customer")]
public record AwsMeteringmarketplaceResolveCustomerOptions(
[property: CommandSwitch("--registration-token")] string RegistrationToken
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}