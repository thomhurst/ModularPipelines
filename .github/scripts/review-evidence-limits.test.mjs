import assert from 'node:assert/strict';
import { spawnSync } from 'node:child_process';
import { mkdtempSync, readFileSync, rmSync, writeFileSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import test from 'node:test';
import { fileURLToPath } from 'node:url';
import { buildReview } from './post-claude-review.mjs';

const script = fileURLToPath(new URL('./review-evidence-limits.mjs', import.meta.url));

function evidenceLimits(t, paths) {
  const directory = mkdtempSync(join(tmpdir(), 'review-evidence-'));
  t.after(() => rmSync(directory, { recursive: true, force: true }));
  const changedFiles = join(directory, 'changed-files.nul');
  const output = join(directory, 'output');
  writeFileSync(changedFiles, paths.length === 0 ? '' : `${paths.join('\0')}\0`);
  const result = spawnSync(process.execPath, [script, changedFiles], {
    env: { ...process.env, GITHUB_OUTPUT: output }, encoding: 'utf8', shell: false,
  });
  assert.equal(result.status, 0, result.stderr);
  const outputs = Object.fromEntries(readFileSync(output, 'utf8').trim().split('\n').map(line => line.split('=')));
  return { minimum: Number(outputs.minimum_evidence_count), maximum: Number(outputs.maximum_evidence_count) };
}

test('an empty captured diff permits exactly zero evidence entries', t => {
  assert.deepEqual(evidenceLimits(t, []), { minimum: 0, maximum: 0 });
});

test('the one-file duplicate-evidence failure remains rejected by the publisher', t => {
  const path = 'test/ModularPipelines.UnitTests/Context/HttpTests.cs';
  const limits = evidenceLimits(t, [path]);
  const rejectedEvidence = [
    { path, assessment: 'Verified the fake clock advances only after the synchronous read starts.' },
    { path, assessment: 'Verified the fake stream honors its configured EOF-on-disposal behavior.' },
  ];
  assert.deepEqual(limits, { minimum: 1, maximum: 1 });
  assert.ok(rejectedEvidence.length > limits.maximum);
  assert.throws(() => buildReview(JSON.stringify({
    summary: 'Reviewed the deterministic synchronous HTTP response-body tests.',
    findings: [], notes: [], evidence: rejectedEvidence,
  }), 'a'.repeat(40), [path]), /distinct files/);
});

test('multiple files use distinct NUL-delimited paths, including unusual names', t => {
  assert.deepEqual(evidenceLimits(t, ['src/a.cs', 'src/line\nbreak.cs', 'src/$(command).cs', 'src/a.cs']),
    { minimum: 1, maximum: 3 });
});
