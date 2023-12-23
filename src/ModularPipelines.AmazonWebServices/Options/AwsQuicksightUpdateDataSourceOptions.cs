using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "update-data-source")]
public record AwsQuicksightUpdateDataSourceOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--data-source-id")] string DataSourceId,
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--data-source-parameters")]
    public string? DataSourceParameters { get; set; }

    [CommandSwitch("--credentials")]
    public string? Credentials { get; set; }

    [CommandSwitch("--vpc-connection-properties")]
    public string? VpcConnectionProperties { get; set; }

    [CommandSwitch("--ssl-properties")]
    public string? SslProperties { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}