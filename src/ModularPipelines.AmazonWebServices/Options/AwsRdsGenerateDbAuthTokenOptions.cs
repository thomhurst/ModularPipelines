using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "generate-db-auth-token")]
public record AwsRdsGenerateDbAuthTokenOptions(
[property: CliOption("--hostname")] string Hostname,
[property: CliOption("--port")] int Port,
[property: CliOption("--username")] string Username
) : AwsOptions;