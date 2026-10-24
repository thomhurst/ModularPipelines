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

if (process.argv[1] && import.meta.url === pathToFileURL(process.argv[1]).href) {
  const cacheHome = preparePowerShellCache(process.env.RUNNER_TEMP);
  const results = await verifyPowerShell({ ...process.env, XDG_CACHE_HOME: cacheHome });
  console.log(results[0]);
  appendFileSync(process.env.GITHUB_ENV, `XDG_CACHE_HOME=${cacheHome}\n`);
}
