using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-sap", "get-database")]
public record AwsSsmSapGetDatabaseOptions : AwsOptions
{
    [CommandSwitch("--application-id")]
    public string? ApplicationId { get; set; }

    [CommandSwitch("--component-id")]
    public string? ComponentId { get; set; }

    [CommandSwitch("--database-id")]
    public string? DatabaseId { get; set; }

    [CommandSwitch("--database-arn")]
    public string? DatabaseArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}