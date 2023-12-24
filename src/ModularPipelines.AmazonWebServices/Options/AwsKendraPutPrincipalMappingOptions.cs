using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "put-principal-mapping")]
public record AwsKendraPutPrincipalMappingOptions(
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--group-id")] string GroupId,
[property: CommandSwitch("--group-members")] string GroupMembers
) : AwsOptions
{
    [CommandSwitch("--data-source-id")]
    public string? DataSourceId { get; set; }

    [CommandSwitch("--ordering-id")]
    public long? OrderingId { get; set; }

    [CommandSwitch("--role-arn")]
    public string? RoleArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}