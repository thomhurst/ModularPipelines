using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("alloydb", "users", "set-password")]
public record GcloudAlloydbUsersSetPasswordOptions(
[property: CliArgument] string Username,
[property: CliOption("--cluster")] string Cluster,
[property: CliOption("--password")] string Password,
[property: CliOption("--region")] string Region
) : GcloudOptions;