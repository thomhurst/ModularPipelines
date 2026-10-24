import assert from 'node:assert/strict';
import { chmodSync, mkdirSync, mkdtempSync, readFileSync, rmSync, statSync, writeFileSync } from 'node:fs';
import os from 'node:os';
import path from 'node:path';
import test from 'node:test';
import { preparePowerShellCache, verifyPowerShell } from './powershell-cache.mjs';

test('each job gets empty, read-only startup profiles without reusing a poisoned cache', t => {
  const root = mkdtempSync(path.join(os.tmpdir(), 'powershell-cache-test-'));
  const profiles = [];
  t.after(() => {
    // Windows requires clearing read-only before removing the test files.
    for (const profile of profiles) chmodSync(profile, 0o600);
    rmSync(root, { recursive: true, force: true });
  });
  const existingCache = path.join(root, 'powershell');
  mkdirSync(existingCache);
  const poisonedProfile = path.join(existingCache, 'StartupProfileData-NonInteractive');
  writeFileSync(poisonedProfile, 'corrupted profile');

  const first = preparePowerShellCache(root);
  const second = preparePowerShellCache(root);
  assert.notEqual(first, second);
  for (const cacheHome of [first, second]) {
    assert.equal(path.dirname(cacheHome), root);
    for (const mode of ['Interactive', 'NonInteractive']) {
      const profile = path.join(cacheHome, 'powershell', `StartupProfileData-${mode}`);
      profiles.push(profile);
      assert.equal(readFileSync(profile).length, 0);
      assert.equal(statSync(profile).mode & 0o222, 0);
    }
  }
  assert.equal(readFileSync(poisonedProfile, 'utf8'), 'corrupted profile');
});

test('startup probes overlap and inherit the protected cache', { timeout: 5000 }, async () => {
  const environment = { XDG_CACHE_HOME: '/protected/cache', PATH: '/tools' };
  let started = 0;
  let release;
  const ready = new Promise(resolve => { release = resolve; });
  const results = await verifyPowerShell(environment, async (tool, args, options) => {
    assert.equal(tool, 'pwsh');
    assert.deepEqual(args.slice(0, 4), ['-NoLogo', '-NoProfile', '-NonInteractive', '-Command']);
    assert.match(args[4], /System.Runtime.Numerics/);
    assert.equal(options.env, environment);
    assert.equal(options.timeout, 30_000);
    if (++started === 4) release();
    await ready;
    return { stdout: 'assembly\nversion\nPOWERSHELL_STARTUP_OK\n' };
  });
  assert.equal(results.length, 4);
});

test('a PowerShell startup abort fails validation', async () => {
  const abort = Object.assign(new Error('PowerShell aborted'), { code: 134 });
  await assert.rejects(verifyPowerShell({}, async () => { throw abort; }), error => error === abort);
});

test('a missing completion marker fails validation even on exit zero', async () => {
  await assert.rejects(verifyPowerShell({}, async () => ({ stdout: 'incomplete startup\n' })), /did not complete/);
});
