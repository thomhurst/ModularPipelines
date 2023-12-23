using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "create-data-source")]
public record AwsQuicksightCreateDataSourceOptions(
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--data-source-id")] string DataSourceId,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--data-source-parameters")]
    public string? DataSourceParameters { get; set; }

    [CommandSwitch("--credentials")]
    public string? Credentials { get; set; }

    [CommandSwitch("--permissions")]
    public string[]? Permissions { get; set; }

    [CommandSwitch("--vpc-connection-properties")]
    public string? VpcConnectionProperties { get; set; }

    [CommandSwitch("--ssl-properties")]
    public string? SslProperties { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--folder-arns")]
    public string[]? FolderArns { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}