import assert from 'node:assert/strict';
import { spawnSync } from 'node:child_process';
import { mkdtempSync, readFileSync, rmSync, writeFileSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import test from 'node:test';
import { fileURLToPath } from 'node:url';

const script = fileURLToPath(new URL('./review-evidence-limits.mjs', import.meta.url));
const workflow = readFileSync(new URL('../workflows/claude-code-review.yml', import.meta.url), 'utf8');

function evidenceSchema(t, paths) {
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
  const schema = workflow.match(/--json-schema '([^']+)'/)[1].replace(
    /\$\{\{ steps\.review_context\.outputs\.(\w+) \}\}/g,
    (_, name) => outputs[name]);
  return JSON.parse(schema).properties.evidence;
}

test('an empty captured diff permits exactly zero evidence entries', t => {
  const schema = evidenceSchema(t, []);
  assert.equal(schema.minItems, 0);
  assert.equal(schema.maxItems, 0);
});

test('the one-file duplicate-evidence failure is rejected by the generation schema', t => {
  const path = 'test/ModularPipelines.UnitTests/Context/HttpTests.cs';
  const schema = evidenceSchema(t, [path]);
  const rejectedEvidence = [
    { path, assessment: 'Verified the fake clock advances only after the synchronous read starts.' },
    { path, assessment: 'Verified the fake stream honors its configured EOF-on-disposal behavior.' },
  ];
  assert.equal(schema.minItems, 1);
  assert.equal(schema.maxItems, 1);
  assert.ok(rejectedEvidence.length > schema.maxItems);
});

test('multiple files use distinct NUL-delimited paths, including unusual names', t => {
  const schema = evidenceSchema(t, ['src/a.cs', 'src/line\nbreak.cs', 'src/$(command).cs', 'src/a.cs']);
  assert.equal(schema.minItems, 1);
  assert.equal(schema.maxItems, 3);
});
