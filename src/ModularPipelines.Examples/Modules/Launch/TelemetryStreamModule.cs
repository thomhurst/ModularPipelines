using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Examples.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.Launch;

[DependsOn<LaunchModule>]
[ModuleCategory("Launch")]
public class TelemetryStreamModule : Module<TelemetryData>
{
    protected override async Task<TelemetryData?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var launch = (await context.GetModule<LaunchModule>()).ValueOrDefault!;

        if (!launch.Successful)
        {
            context.Logger.LogWarning("No telemetry available - launch was not successful");
            return new TelemetryData(0, 0, 0, TimeSpan.Zero);
        }

        context.Logger.LogInformation("Initiating telemetry stream from vehicle {MissionId}...", launch.MissionId);
        context.Logger.LogDebug("Configuring telemetry downlink decoder...");
        context.Logger.LogDebug("Telemetry rate: 1 Mbps");
        context.Logger.LogDebug("Frame sync acquired");

        // Simulate telemetry updates with SubModules
        await context.SubModule("Telemetry T+30s", async () =>
        {
            context.Logger.LogInformation("T+30s: First stage powered flight");
            context.Logger.LogDebug("Altitude: 15 km");
            context.Logger.LogDebug("Velocity: 450 m/s");
            context.Logger.LogDebug("Acceleration: 2.1 G");
            context.Logger.LogDebug("Engine 1: 101% thrust");
            context.Logger.LogDebug("Engine 2: 100% thrust");
            context.Logger.LogDebug("Engine 3: 101% thrust");
            context.Logger.LogDebug("Fuel remaining: 78%");
            context.Logger.LogDebug("Trajectory deviation: 0.02°");
            await Task.Delay(TimeSpan.FromMilliseconds(400), cancellationToken);
            return true;
        });

        await context.SubModule("Telemetry T+60s", async () =>
        {
            context.Logger.LogInformation("T+60s: Approaching Max-Q");
            context.Logger.LogDebug("Altitude: 50 km");
            context.Logger.LogDebug("Velocity: 1,200 m/s");
            context.Logger.LogDebug("Dynamic pressure: 32,500 Pa");
            context.Logger.LogDebug("Throttle setting: 72%");
            context.Logger.LogDebug("Vehicle structural loads: NOMINAL");
            context.Logger.LogDebug("Fuel remaining: 52%");
            context.Logger.LogDebug("Downrange distance: 45 km");
            await Task.Delay(TimeSpan.FromMilliseconds(400), cancellationToken);
            return true;
        });

        await context.SubModule("Telemetry T+90s", async () =>
        {
            context.Logger.LogInformation("T+90s: MECO - Main Engine Cutoff");
            context.Logger.LogDebug("Altitude: 120 km");
            context.Logger.LogDebug("Velocity: 2,800 m/s");
            context.Logger.LogDebug("All three engines shutdown confirmed");
            context.Logger.LogDebug("First stage performance: NOMINAL");
            context.Logger.LogDebug("First stage fuel depleted: 99.8%");
            context.Logger.LogDebug("Stage separation pyros: ARMED");
            await Task.Delay(TimeSpan.FromMilliseconds(400), cancellationToken);
            return true;
        });

        await context.SubModule("Telemetry T+120s", async () =>
        {
            context.Logger.LogInformation("T+120s: Stage separation and S2 ignition");
            context.Logger.LogDebug("Stage separation confirmed");
            context.Logger.LogDebug("First stage entering ballistic trajectory");
            context.Logger.LogDebug("Second stage ignition: CONFIRMED");
            context.Logger.LogDebug("Altitude: 200 km");
            context.Logger.LogDebug("Velocity: 5,500 m/s");
            context.Logger.LogDebug("S2 engine performance: 100%");
            context.Logger.LogDebug("Payload fairing separation: NOMINAL");
            context.Logger.LogDebug("Fairing halves tracking confirmed");
            await Task.Delay(TimeSpan.FromMilliseconds(400), cancellationToken);
            return true;
        });

        var missionElapsed = DateTime.UtcNow - launch.LaunchTime;

        context.Logger.LogInformation("Telemetry stream complete - vehicle in stable orbit");
        context.Logger.LogDebug("Final orbit: 400 x 402 km, 28.5° inclination");
        context.Logger.LogDebug("Orbital velocity: 7,800 m/s");
        context.Logger.LogDebug("Fuel remaining in S2: 12.5%");
        context.Logger.LogDebug("Mission elapsed time: {Elapsed}", missionElapsed.ToString(@"mm\:ss\.fff"));
        context.Logger.LogDebug("All systems nominal for orbital operations");

        return new TelemetryData(
            AltitudeKm: 400.0,
            VelocityMps: 7800.0,
            FuelRemaining: 12.5,
            MissionElapsed: missionElapsed);
    }
}
