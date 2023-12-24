using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "put-profile-object")]
public record AwsCustomerProfilesPutProfileObjectOptions(
[property: CommandSwitch("--object-type-name")] string ObjectTypeName,
[property: CommandSwitch("--object")] string Object,
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}