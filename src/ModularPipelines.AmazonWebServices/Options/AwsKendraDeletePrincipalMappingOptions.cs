using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("kendra", "delete-principal-mapping")]
public record AwsKendraDeletePrincipalMappingOptions(
[property: CommandSwitch("--index-id")] string IndexId,
[property: CommandSwitch("--group-id")] string GroupId
) : AwsOptions
{
    [CommandSwitch("--data-source-id")]
    public string? DataSourceId { get; set; }

    [CommandSwitch("--ordering-id")]
    public long? OrderingId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}