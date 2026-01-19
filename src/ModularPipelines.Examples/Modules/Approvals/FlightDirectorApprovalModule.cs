using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Examples.Models;
using ModularPipelines.Examples.Modules.PreFlight;
using ModularPipelines.Examples.Modules.Systems;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.Approvals;

[DependsOn<WeatherAnalysisModule>]
[DependsOn<CrewReadinessModule>]
[DependsOn<NavigationSystemModule>]
[DependsOn<LifeSupportCheckModule>]
[DependsOn<CommunicationsCheckModule>]
[DependsOn<FuelSystemCheckModule>]
[ModuleCategory("Approvals")]
public class FlightDirectorApprovalModule : Module<ApprovalResult>
{
    protected override async Task<ApprovalResult?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        context.Logger.LogInformation("Flight Director initiating final launch readiness review...");
        context.Logger.LogDebug("Polling all console positions for GO/NO-GO status...");

        // Retrieve all dependency results
        var weather = (await context.GetModule<WeatherAnalysisModule>()).ValueOrDefault!;
        var crew = (await context.GetModule<CrewReadinessModule>()).ValueOrDefault!;
        var nav = (await context.GetModule<NavigationSystemModule>()).ValueOrDefault!;
        var lifeSupport = (await context.GetModule<LifeSupportCheckModule>()).ValueOrDefault!;
        var comms = (await context.GetModule<CommunicationsCheckModule>()).ValueOrDefault!;
        var fuel = (await context.GetModule<FuelSystemCheckModule>()).ValueOrDefault!;

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("Reviewing weather station report...");
        context.Logger.LogDebug("Weather: Temp {Temp}F, Wind {Wind}mph, Visibility {Vis}mi",
            weather.Temperature, weather.WindSpeed, weather.Visibility);

        // Make decisions based on aggregated data
        if (!weather.IsLaunchSafe)
        {
            return new ApprovalResult(false, "Flight Director", DateTime.UtcNow,
                $"Weather unsafe: wind {weather.WindSpeed}mph");
        }

        context.Logger.LogDebug("WEATHER: GO");

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("Reviewing crew status...");
        context.Logger.LogDebug("Crew: {Count} members, Health: {Health}, Training: {Training}",
            crew.CrewCount, crew.AllHealthy ? "OK" : "CONCERN", crew.AllTrained ? "CURRENT" : "EXPIRED");

        if (!crew.AllHealthy || !crew.AllTrained)
        {
            return new ApprovalResult(false, "Flight Director", DateTime.UtcNow, "Crew not ready");
        }

        context.Logger.LogDebug("SURGEON: GO");
        context.Logger.LogDebug("CREW: GO");

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("Reviewing navigation systems...");
        context.Logger.LogDebug("Navigation: GPS {Lock}, Heading {Heading}",
            nav.GpsLocked ? "LOCKED" : "NO LOCK", nav.Heading);

        if (!nav.GpsLocked)
        {
            return new ApprovalResult(false, "Flight Director", DateTime.UtcNow, "GPS not locked");
        }

        context.Logger.LogDebug("GNC: GO");

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("Reviewing life support systems...");
        context.Logger.LogDebug("Life Support: O2 {O2}%, CO2 {CO2}%, Pressure {P}psi",
            lifeSupport.OxygenLevel, lifeSupport.CO2Level, lifeSupport.CabinPressure);

        if (!lifeSupport.Nominal)
        {
            return new ApprovalResult(false, "Flight Director", DateTime.UtcNow, "Life support anomaly");
        }

        context.Logger.LogDebug("EECOM: GO");

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("Reviewing communications status...");
        context.Logger.LogDebug("Comms: Uplink {Up}, Downlink {Down}, BW {Bw}Mbps",
            comms.UplinkActive ? "ACTIVE" : "DOWN", comms.DownlinkActive ? "ACTIVE" : "DOWN", comms.Bandwidth);

        if (!comms.UplinkActive || !comms.DownlinkActive)
        {
            return new ApprovalResult(false, "Flight Director", DateTime.UtcNow, "Communications offline");
        }

        context.Logger.LogDebug("INCO: GO");

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("Reviewing propellant status...");
        context.Logger.LogDebug("Fuel: {Fuel}%, Oxidizer: {Ox}%, Pressure: {P}",
            fuel.FuelLevel, fuel.OxidizerLevel, fuel.PressureNominal ? "NOMINAL" : "OFF-NOMINAL");

        if (fuel.FuelLevel < 95)
        {
            return new ApprovalResult(false, "Flight Director", DateTime.UtcNow,
                $"Insufficient fuel: {fuel.FuelLevel}%");
        }

        context.Logger.LogDebug("PROP: GO");
        context.Logger.LogDebug("BOOSTER: GO");
        context.Logger.LogDebug("TRAJECTORY: GO");
        context.Logger.LogDebug("RANGE: GO");
        context.Logger.LogDebug("FAO: GO");

        await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
        context.Logger.LogInformation("Flight Director: All stations report GO for launch");
        context.Logger.LogDebug("Recording approval timestamp...");

        context.Logger.LogInformation("All systems nominal - FLIGHT DIRECTOR APPROVAL GRANTED");
        return new ApprovalResult(true, "Flight Director", DateTime.UtcNow, null);
    }
}
