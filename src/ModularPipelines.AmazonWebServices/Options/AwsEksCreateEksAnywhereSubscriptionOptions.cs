using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("eks", "create-eks-anywhere-subscription")]
public record AwsEksCreateEksAnywhereSubscriptionOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--term")] string Term
) : AwsOptions
{
    [CommandSwitch("--license-quantity")]
    public int? LicenseQuantity { get; set; }

    [CommandSwitch("--license-type")]
    public string? LicenseType { get; set; }

    [CommandSwitch("--client-request-token")]
    public string? ClientRequestToken { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}