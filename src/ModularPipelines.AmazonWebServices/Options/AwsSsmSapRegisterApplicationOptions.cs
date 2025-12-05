using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm-sap", "register-application")]
public record AwsSsmSapRegisterApplicationOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--application-type")] string ApplicationType,
[property: CliOption("--instances")] string[] Instances
) : AwsOptions
{
    [CliOption("--sap-instance-number")]
    public string? SapInstanceNumber { get; set; }

    [CliOption("--sid")]
    public string? Sid { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--credentials")]
    public string[]? Credentials { get; set; }

    [CliOption("--database-arn")]
    public string? DatabaseArn { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}