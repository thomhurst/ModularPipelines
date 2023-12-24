using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift-serverless", "create-workgroup")]
public record AwsRedshiftServerlessCreateWorkgroupOptions(
[property: CommandSwitch("--namespace-name")] string NamespaceName,
[property: CommandSwitch("--workgroup-name")] string WorkgroupName
) : AwsOptions
{
    [CommandSwitch("--base-capacity")]
    public int? BaseCapacity { get; set; }

    [CommandSwitch("--config-parameters")]
    public string[]? ConfigParameters { get; set; }

    [CommandSwitch("--max-capacity")]
    public int? MaxCapacity { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--subnet-ids")]
    public string[]? SubnetIds { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}