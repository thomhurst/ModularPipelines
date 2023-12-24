using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-contacts", "create-contact-channel")]
public record AwsSsmContactsCreateContactChannelOptions(
[property: CommandSwitch("--contact-id")] string ContactId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--type")] string Type,
[property: CommandSwitch("--delivery-address")] string DeliveryAddress
) : AwsOptions
{
    [CommandSwitch("--idempotency-token")]
    public string? IdempotencyToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}