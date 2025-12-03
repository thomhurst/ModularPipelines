using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "reservations", "get-iam-policy")]
public record GcloudComputeReservationsGetIamPolicyOptions(
[property: CliArgument] string Reservation,
[property: CliArgument] string Zone
) : GcloudOptions;