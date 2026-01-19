using Microsoft.Extensions.Logging;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Examples.Models;
using ModularPipelines.Examples.Modules.PreFlight;
using ModularPipelines.Modules;

namespace ModularPipelines.Examples.Modules.Systems;

[DependsOn<SatelliteTrackingModule>]
[ModuleCategory("Systems")]
public class NavigationSystemModule : Module<NavigationData>
{
    protected override async Task<NavigationData?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var satelliteModule = await context.GetModule<SatelliteTrackingModule>();
        var satellite = satelliteModule.ValueOrDefault!;

        context.Logger.LogInformation("Initializing navigation system using satellite {Id} with signal {Signal}dB...",
            satellite.SatelliteId, satellite.SignalStrength);
        context.Logger.LogDebug("Loading navigation software version 4.2.1...");

        await Task.Delay(TimeSpan.FromMilliseconds(200), cancellationToken);
        context.Logger.LogInformation("Acquiring GPS constellation fix...");
        context.Logger.LogDebug("GPS SV PRN-01: Acquired, C/N0 45 dB-Hz");
        context.Logger.LogDebug("GPS SV PRN-03: Acquired, C/N0 42 dB-Hz");
        await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken);
        context.Logger.LogDebug("GPS SV PRN-06: Acquired, C/N0 44 dB-Hz");
        context.Logger.LogDebug("GPS SV PRN-11: Acquired, C/N0 41 dB-Hz");
        context.Logger.LogDebug("GPS SV PRN-17: Acquired, C/N0 43 dB-Hz");
        await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken);
        context.Logger.LogDebug("GPS SV PRN-22: Acquired, C/N0 40 dB-Hz");
        context.Logger.LogDebug("GPS SV PRN-28: Acquired, C/N0 46 dB-Hz");
        context.Logger.LogDebug("7 satellites tracked - 3D fix achieved");

        context.Logger.LogInformation("Computing precise position solution...");
        await Task.Delay(TimeSpan.FromMilliseconds(200), cancellationToken);
        context.Logger.LogDebug("GDOP: 1.8 - EXCELLENT");
        context.Logger.LogDebug("PDOP: 1.5 - EXCELLENT");
        context.Logger.LogDebug("Position accuracy: 2.1m CEP");
        context.Logger.LogDebug("Velocity accuracy: 0.05 m/s RMS");

        context.Logger.LogInformation("Initializing inertial navigation unit...");
        await Task.Delay(TimeSpan.FromMilliseconds(200), cancellationToken);
        context.Logger.LogDebug("Ring laser gyro X: CALIBRATED");
        context.Logger.LogDebug("Ring laser gyro Y: CALIBRATED");
        context.Logger.LogDebug("Ring laser gyro Z: CALIBRATED");
        await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken);
        context.Logger.LogDebug("Accelerometer triad: CALIBRATED");
        context.Logger.LogDebug("INS/GPS filter: CONVERGED");
        context.Logger.LogDebug("Navigation mode: HYBRID INS/GPS");

        context.Logger.LogInformation("Loading launch site coordinates...");
        await Task.Delay(TimeSpan.FromMilliseconds(150), cancellationToken);
        context.Logger.LogDebug("Launch pad: LC-39A");
        context.Logger.LogDebug("Latitude: 28.5721°N");
        context.Logger.LogDebug("Longitude: 80.6480°W");
        context.Logger.LogDebug("Altitude: 3m MSL");
        context.Logger.LogDebug("Magnetic declination: -5.2°");

        var navData = new NavigationData(
            Latitude: 28.5721,
            Longitude: -80.6480,
            Heading: satellite.SignalStrength > 50 ? 90.0 : 89.5,
            GpsLocked: satellite.Locked);

        context.Logger.LogInformation("Navigation: Lat {Lat}, Lon {Lon}, Heading {Head}, GPS: {Gps}",
            navData.Latitude,
            navData.Longitude,
            navData.Heading,
            navData.GpsLocked ? "LOCKED" : "SEARCHING");

        return navData;
    }
}
