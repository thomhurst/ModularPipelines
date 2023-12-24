using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("docdb", "delete-db-instance")]
public record AwsDocdbDeleteDbInstanceOptions(
[property: CommandSwitch("--db-instance-identifier")] string DbInstanceIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}