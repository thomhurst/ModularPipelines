using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("customer-profiles", "put-integration")]
public record AwsCustomerProfilesPutIntegrationOptions(
[property: CommandSwitch("--domain-name")] string DomainName
) : AwsOptions
{
    [CommandSwitch("--uri")]
    public string? Uri { get; set; }

    [CommandSwitch("--object-type-name")]
    public string? ObjectTypeName { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--flow-definition")]
    public string? FlowDefinition { get; set; }

    [CommandSwitch("--object-type-names")]
    public IEnumerable<KeyValue>? ObjectTypeNames { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}