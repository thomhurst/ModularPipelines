using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cleanrooms", "create-configured-table-association")]
public record AwsCleanroomsCreateConfiguredTableAssociationOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--membership-identifier")] string MembershipIdentifier,
[property: CommandSwitch("--configured-table-identifier")] string ConfiguredTableIdentifier,
[property: CommandSwitch("--role-arn")] string RoleArn
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}