using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ssm-sap", "register-application")]
public record AwsSsmSapRegisterApplicationOptions(
[property: CommandSwitch("--application-id")] string ApplicationId,
[property: CommandSwitch("--application-type")] string ApplicationType,
[property: CommandSwitch("--instances")] string[] Instances
) : AwsOptions
{
    [CommandSwitch("--sap-instance-number")]
    public string? SapInstanceNumber { get; set; }

    [CommandSwitch("--sid")]
    public string? Sid { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--credentials")]
    public string[]? Credentials { get; set; }

    [CommandSwitch("--database-arn")]
    public string? DatabaseArn { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}