using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm", "delete-association")]
public record AwsSsmDeleteAssociationOptions : AwsOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--instance-id")]
    public string? InstanceId { get; set; }

    [CommandSwitch("--association-id")]
    public string? AssociationId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}