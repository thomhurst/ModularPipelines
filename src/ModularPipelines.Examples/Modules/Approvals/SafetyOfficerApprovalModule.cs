using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Examples.Models;
using ModularPipelines.Examples.Modules.PreFlight;
using ModularPipelines.Examples.Modules.Propulsion;
using ModularPipelines.Examples.Modules.Systems;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.Approvals;

[DependsOn<WeatherAnalysisModule>]
[DependsOn<LifeSupportCheckModule>]
[DependsOn<CommunicationsCheckModule>]
[DependsOn<LaunchTrajectoryCalculationModule>]
[ModuleCategory("Approvals")]
public class SafetyOfficerApprovalModule : Module<ApprovalResult>
{
    protected override async Task<ApprovalResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Safety Officer initiating critical systems safety review...");
        context.Logger.LogDebug("Loading safety constraint parameters...");

        // Retrieve all dependency results
        var weather = (await context.GetModule<WeatherAnalysisModule>()).ValueOrDefault!;
        var lifeSupport = (await context.GetModule<LifeSupportCheckModule>()).ValueOrDefault!;
        var comms = (await context.GetModule<CommunicationsCheckModule>()).ValueOrDefault!;
        var trajectory = (await context.GetModule<LaunchTrajectoryCalculationModule>()).ValueOrDefault!;

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("Evaluating weather safety constraints...");
        context.Logger.LogDebug("Wind speed: {Wind}mph (limit: 30mph)", weather.WindSpeed);
        context.Logger.LogDebug("Visibility: {Vis}mi (minimum: 3mi)", weather.Visibility);
        context.Logger.LogDebug("Lightning within 10nm: NEGATIVE");

        // Make safety-focused decisions
        if (!weather.IsLaunchSafe)
        {
            return new ApprovalResult(false, "Safety Officer", DateTime.UtcNow,
                $"Weather conditions unsafe: visibility {weather.Visibility}mi");
        }

        context.Logger.LogDebug("Weather safety: ACCEPTABLE");

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("Evaluating crew safety systems...");
        context.Logger.LogDebug("O2 partial pressure: {O2}% (range: 19.5-23.5%)", lifeSupport.OxygenLevel);
        context.Logger.LogDebug("CO2 level: {CO2}% (limit: 0.5%)", lifeSupport.CO2Level);
        context.Logger.LogDebug("Cabin pressure: {P}psi (range: 14.0-15.0)", lifeSupport.CabinPressure);

        if (!lifeSupport.Nominal)
        {
            return new ApprovalResult(false, "Safety Officer", DateTime.UtcNow,
                $"Life support anomaly: O2 {lifeSupport.OxygenLevel}%, CO2 {lifeSupport.CO2Level}%");
        }

        context.Logger.LogDebug("Life support safety: ACCEPTABLE");
        context.Logger.LogDebug("Crew escape system: ARMED");
        context.Logger.LogDebug("Pad abort mode: READY");

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("Verifying abort capability...");
        context.Logger.LogDebug("Uplink command capability: {Up}", comms.UplinkActive ? "CONFIRMED" : "LOST");
        context.Logger.LogDebug("Downlink telemetry: {Down}", comms.DownlinkActive ? "NOMINAL" : "DEGRADED");
        context.Logger.LogDebug("Flight termination system: ARMED");

        if (!comms.UplinkActive || !comms.DownlinkActive)
        {
            return new ApprovalResult(false, "Safety Officer", DateTime.UtcNow,
                "Communications not fully operational - abort capability compromised");
        }

        context.Logger.LogDebug("Abort capability: VERIFIED");

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("Reviewing range safety parameters...");
        context.Logger.LogDebug("Trajectory azimuth: {Az}° (corridor: 85-95°)", trajectory.Azimuth);
        context.Logger.LogDebug("Impact zone: Atlantic Ocean, exclusion zone CLEAR");
        context.Logger.LogDebug("Tracking radar: LOCKED");

        if (!trajectory.Calculated)
        {
            return new ApprovalResult(false, "Safety Officer", DateTime.UtcNow,
                "Trajectory not calculated - flight path unknown");
        }

        context.Logger.LogDebug("Range safety: ACCEPTABLE");

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("Final safety checklist verification...");
        context.Logger.LogDebug("Emergency response teams: STATIONED");
        context.Logger.LogDebug("Medical evacuation helicopter: ON STANDBY");
        context.Logger.LogDebug("Fire suppression systems: READY");
        context.Logger.LogDebug("Public safety perimeter: SECURED");

        context.Logger.LogInformation("All safety checks passed - SAFETY OFFICER APPROVAL GRANTED");
        return new ApprovalResult(true, "Safety Officer", DateTime.UtcNow, null);
    }
}
