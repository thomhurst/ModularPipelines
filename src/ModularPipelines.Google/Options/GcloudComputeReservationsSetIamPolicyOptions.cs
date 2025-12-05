using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "reservations", "set-iam-policy")]
public record GcloudComputeReservationsSetIamPolicyOptions(
[property: CliArgument] string Reservation,
[property: CliArgument] string Zone,
[property: CliArgument] string PolicyFile
) : GcloudOptions;