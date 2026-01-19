using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Examples.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.Launch;

[DependsOn<FinalCountdownModule>]
[ModuleCategory("Launch")]
public class LaunchModule : Module<LaunchResult>
{
    protected override async Task<LaunchResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var countdown = (await context.GetModule<FinalCountdownModule>()).ValueOrDefault!;

        if (countdown.HoldCalled)
        {
            context.Logger.LogError("Launch scrubbed - countdown hold was called");
            return new LaunchResult(false, DateTime.MinValue, "SCRUBBED");
        }

        context.Logger.LogInformation("LIFTOFF CONFIRMED - Vehicle has cleared the tower!");
        context.Logger.LogDebug("All hold-down clamps released");
        context.Logger.LogDebug("Umbilical disconnected");

        await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken);
        context.Logger.LogInformation("Tower clear at T+7 seconds");
        context.Logger.LogDebug("Beginning pitch and roll maneuver...");
        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogDebug("Roll program initiated");
        context.Logger.LogDebug("Vehicle rolling to flight azimuth 90°");

        await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken);
        context.Logger.LogInformation("T+15: Roll program complete");
        context.Logger.LogDebug("Now heads-down, wings level");
        context.Logger.LogDebug("Transitioning to gravity turn");

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("T+25: Velocity 500 m/s, Altitude 5 km");
        context.Logger.LogDebug("All engines nominal");
        context.Logger.LogDebug("Guidance is GO");

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("T+45: Approaching maximum dynamic pressure");
        context.Logger.LogDebug("Throttling back to 65% for Max-Q");
        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogDebug("Max-Q: 35,000 Pa");
        context.Logger.LogDebug("Vehicle structural loads nominal");

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("T+65: Max-Q passed - throttling up");
        context.Logger.LogDebug("Engines returning to full thrust");
        context.Logger.LogDebug("Velocity 1,200 m/s, Altitude 25 km");

        var launchTime = DateTime.UtcNow;
        var missionId = $"MISSION-{launchTime:yyyyMMdd-HHmmss}";

        context.Logger.LogInformation("Launch phase complete - entering sustained flight");
        context.Logger.LogDebug("Mission ID assigned: {MissionId}", missionId);
        context.Logger.LogDebug("Nominal trajectory confirmed");
        context.Logger.LogDebug("Downrange tracking stations acquired");

        context.Logger.LogInformation("Launch successful! Mission ID: {MissionId}", missionId);

        return new LaunchResult(
            Successful: true,
            LaunchTime: launchTime,
            MissionId: missionId);
    }
}
