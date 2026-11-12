import { execFile } from 'node:child_process';
import { appendFileSync, chmodSync, mkdirSync, mkdtempSync, writeFileSync } from 'node:fs';
import path from 'node:path';
import { promisify } from 'node:util';
import { pathToFileURL } from 'node:url';

export function preparePowerShellCache(runnerTemp) {
  const cacheHome = mkdtempSync(path.join(runnerTemp, 'powershell-cache-'));
  const cache = path.join(cacheHome, 'powershell');
  mkdirSync(cache);
  // Parallel pwsh sessions can corrupt their shared multicore-JIT startup profile.
  // -NoProfile does not disable it. Use fresh, read-only files, as documented in
  // https://github.com/PowerShell/PowerShell/issues/26528 (dotnet/runtime#121977).
  for (const mode of ['Interactive', 'NonInteractive']) {
    const profile = path.join(cache, `StartupProfileData-${mode}`);
    writeFileSync(profile, '', { flag: 'wx', mode: 0o444 });
    chmodSync(profile, 0o444);
  }
  return cacheHome;
}

export function protectPowerShellEnvironment(environment, cacheHome) {
  return {
    ...environment,
    XDG_CACHE_HOME: cacheHome,
    // CoreCLR enables ProfileOptimization only when the CPU count reaches this
    // threshold. DWORD environment settings use hexadecimal: FFFFFFFF disables
    // both playback and gathering on hosted runners, unlike file permissions.
    // https://github.com/dotnet/runtime/blob/v10.0.0/src/coreclr/vm/multicorejit.cpp
    DOTNET_MultiCoreJitMinNumCpus: 'FFFFFFFF',
    // Hosted macOS still segfaults before the first command with profiling off
    // (#7502). Bypass the ReadyToRun loader path as well; upstream stress tests
    // report no crashes with this setting, at the cost of more startup JIT work.
    // https://github.com/PowerShell/PowerShell/issues/26528#issuecomment-6777944180
    DOTNET_ReadyToRun: '0',
  };
}

export async function verifyPowerShell(environment, run = promisify(execFile)) {
  // Exercise the same non-interactive startup as command and installer fixtures,
  // including the assembly whose corrupted name caused #7200. Fail on any abort.
  const command = "$ErrorActionPreference = 'Stop'; "
    + "[System.Reflection.Assembly]::Load('System.Runtime.Numerics').FullName; "
    + "$PSHOME; $PSVersionTable.PSVersion.ToString(); [Environment]::Version.ToString(); "
    + "Write-Output 'POWERSHELL_STARTUP_OK'";
  return await Promise.all(Array.from({ length: 4 }, async () => {
    const result = await run('pwsh', ['-NoLogo', '-NoProfile', '-NonInteractive', '-Command', command], {
      env: environment,
      timeout: 30_000,
      maxBuffer: 64 * 1024,
    });
    if (!result.stdout.split(/\r?\n/).includes('POWERSHELL_STARTUP_OK')) {
      throw new Error('PowerShell startup probe did not complete.');
    }
    return result.stdout;
  }));
}

export async function protectPowerShellStartup(inherited, run) {
  const cacheHome = preparePowerShellCache(inherited.RUNNER_TEMP);
  const environment = protectPowerShellEnvironment(inherited, cacheHome);
  const results = await verifyPowerShell(environment, run);
  // Export the same protection to the worker and the fixture processes it starts.
  appendFileSync(inherited.GITHUB_ENV,
    `XDG_CACHE_HOME=${cacheHome}\nDOTNET_MultiCoreJitMinNumCpus=${environment.DOTNET_MultiCoreJitMinNumCpus}\nDOTNET_ReadyToRun=${environment.DOTNET_ReadyToRun}\n`);
  return results;
}

if (process.argv[1] && import.meta.url === pathToFileURL(process.argv[1]).href) {
  const results = await protectPowerShellStartup(process.env);
  console.log(results[0]);
}
