using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Examples.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.Launch;

[DependsOn<TelemetryStreamModule>]
[DependsOn<LaunchModule>]
[ModuleCategory("Launch")]
public class MissionStatusReportModule : Module<MissionReport>
{
    protected override async Task<MissionReport?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var telemetry = (await context.GetModule<TelemetryStreamModule>()).ValueOrDefault!;
        var launch = (await context.GetModule<LaunchModule>()).ValueOrDefault!;

        context.Logger.LogInformation("Compiling final mission status report for {MissionId}...", launch.MissionId);
        context.Logger.LogDebug("Aggregating telemetry data...");

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("Verifying orbital parameters...");
        context.Logger.LogDebug("Perigee: 398 km");
        context.Logger.LogDebug("Apogee: 402 km");
        context.Logger.LogDebug("Inclination: 28.5°");
        context.Logger.LogDebug("Orbital period: 92.4 minutes");
        context.Logger.LogDebug("Eccentricity: 0.0003");

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("Reviewing vehicle health status...");
        context.Logger.LogDebug("Power systems: NOMINAL - Solar arrays deployed");
        context.Logger.LogDebug("Thermal control: NOMINAL");
        context.Logger.LogDebug("Communications: NOMINAL - Both S-band and Ku-band active");
        context.Logger.LogDebug("Propulsion: NOMINAL - 12.5% fuel remaining for orbital maneuvers");
        context.Logger.LogDebug("Navigation: NOMINAL - GPS lock confirmed in orbit");

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("Documenting mission milestones...");
        context.Logger.LogDebug("T+0: Liftoff");
        context.Logger.LogDebug("T+62s: Max-Q");
        context.Logger.LogDebug("T+168s: MECO");
        context.Logger.LogDebug("T+170s: Stage separation");
        context.Logger.LogDebug("T+178s: S2 ignition");
        context.Logger.LogDebug("T+215s: Fairing separation");
        context.Logger.LogDebug("T+520s: SECO-1");
        context.Logger.LogDebug("T+1810s: SECO-2 and orbit achieved");

        var success = launch.Successful && telemetry.AltitudeKm > 100;
        var report = new MissionReport(
            MissionId: launch.MissionId,
            Success: success,
            TotalDuration: telemetry.MissionElapsed,
            Summary: $"Achieved {telemetry.AltitudeKm}km altitude at {telemetry.VelocityMps}m/s");

        context.Logger.LogInformation("Writing summary to pipeline output...");
        context.Logger.LogDebug("Mission ID: {Id}", report.MissionId);
        context.Logger.LogDebug("Final altitude: {Alt}km", telemetry.AltitudeKm);
        context.Logger.LogDebug("Orbital velocity: {Vel}m/s", telemetry.VelocityMps);
        context.Logger.LogDebug("Mission duration: {Duration}", report.TotalDuration.ToString(@"mm\:ss"));

        // Output to pipeline summary
        context.Summary.KeyValue("Mission", "ID", report.MissionId);
        context.Summary.KeyValue("Mission", "Altitude", $"{telemetry.AltitudeKm}km");
        context.Summary.KeyValue("Mission", "Velocity", $"{telemetry.VelocityMps}m/s");
        context.Summary.KeyValue("Mission", "Duration", report.TotalDuration.ToString(@"mm\:ss"));

        if (report.Success)
        {
            context.Summary.Success("Mission", report.Summary);
            context.Logger.LogInformation("===========================================");
            context.Logger.LogInformation("MISSION SUCCESS: {Summary}", report.Summary);
            context.Logger.LogInformation("===========================================");
            context.Logger.LogDebug("Vehicle is nominal and ready for operational phase");
            context.Logger.LogDebug("Ground stations tracking: Kennedy, Bermuda, Ascension, Diego Garcia");
            context.Logger.LogDebug("Next ground contact window: T+47 minutes");
        }
        else
        {
            context.Summary.Error("Mission", "Launch failed or orbit not achieved");
            context.Logger.LogError("===========================================");
            context.Logger.LogError("MISSION FAILURE: Did not achieve stable orbit");
            context.Logger.LogError("===========================================");
            context.Logger.LogDebug("Initiating failure analysis procedures...");
            context.Logger.LogDebug("Telemetry data preserved for review");
        }

        return report;
    }
}
