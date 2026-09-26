import assert from 'node:assert/strict';
import { spawnSync } from 'node:child_process';
import { mkdtempSync, rmSync, writeFileSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import test from 'node:test';
import { fileURLToPath } from 'node:url';
import { buildReview } from './post-claude-review.mjs';

const headSha = 'a'.repeat(40);
const changedFiles = ['src/example.cs'];
const review = {
  summary: 'Reviewed cancellation handling and error propagation in the changed command.',
  findings: [], notes: [],
  evidence: [{ path: changedFiles[0], assessment: 'Verified cancellation reaches the command and errors propagate to callers.' }],
};

function runHook(t, message, active = false, overrides = {}, input) {
  const directory = mkdtempSync(join(tmpdir(), 'review-stop-'));
  t.after(() => rmSync(directory, { recursive: true, force: true }));
  const pathsFile = join(directory, 'changed-files.nul');
  writeFileSync(pathsFile, changedFiles.join('\0') + '\0');
  return spawnSync(process.execPath, [fileURLToPath(new URL('./validate-claude-review-stop.mjs', import.meta.url))], {
    input: input ?? JSON.stringify({ hook_event_name: 'Stop', stop_hook_active: active, last_assistant_message: message }),
    encoding: 'utf8', shell: false,
    // Run outside the checkout: neither the caller's cwd nor event paths locate trusted code.
    cwd: directory,
    env: { ...process.env, REVIEW_HEAD_SHA: headSha, REVIEW_CHANGED_FILES: pathsFile, ...overrides },
  });
}

test('valid clear and blocking reviews can stop without changing their verdict', t => {
  for (const findings of [[], ['src/example.cs drops cancellation; forward the supplied token.']]) {
    const rawReview = JSON.stringify({ ...review, findings });
    const result = runHook(t, rawReview);
    assert.equal(result.status, 0, result.stderr);
    assert.equal(result.stderr, '');
    assert.deepEqual(JSON.parse(result.stdout), {});
    assert.match(buildReview(rawReview, headSha, changedFiles),
      findings.length === 0 ? /REVIEW_VERDICT: CLEAR/ : /REVIEW_VERDICT: BLOCKING/);
  }
});

test('invalid final text receives one private-content-free correction request', t => {
  for (const text of ['PRIVATE_SENTINEL plain prose', '{PRIVATE_SENTINEL',
    JSON.stringify({ ...review, evidence: [{ path: 'PRIVATE_SENTINEL', assessment: review.evidence[0].assessment }] }),
    JSON.stringify({ ...review, summary: 'PRIVATE_SENTINEL' })]) {
    const result = runHook(t, text);
    assert.equal(result.status, 0, result.stderr);
    assert.equal(result.stderr, '');
    const decision = JSON.parse(result.stdout);
    assert.equal(decision.decision, 'block');
    assert.match(decision.reason, /complete actual review/);
    assert.match(decision.reason, /Preserve every actionable finding/);
    assert.doesNotMatch(result.stdout, /PRIVATE_SENTINEL/);
  }
});

test('a second invalid final response can stop but still fails publisher validation', t => {
  const text = 'PRIVATE_SENTINEL still not JSON';
  const result = runHook(t, text, true);
  assert.equal(result.status, 0, result.stderr);
  assert.equal(result.stderr, '');
  assert.deepEqual(JSON.parse(result.stdout), {});
  assert.throws(() => buildReview(text, headSha, changedFiles), /valid structured review/);
});

test('a corrected review retains actionable findings', t => {
  const rawReview = JSON.stringify({ ...review, findings: ['src/example.cs drops cancellation; forward the supplied token.'] });
  const result = runHook(t, rawReview, true);
  assert.equal(result.status, 0, result.stderr);
  assert.deepEqual(JSON.parse(result.stdout), {});
  assert.match(buildReview(rawReview, headSha, changedFiles), /REVIEW_VERDICT: BLOCKING/);
});

test('hook setup failures produce only a fixed diagnostic', t => {
  const result = runHook(t, 'PRIVATE_SENTINEL', false, { REVIEW_CHANGED_FILES: 'PRIVATE_SENTINEL-missing-file' });
  assert.equal(result.status, 1);
  assert.equal(result.stdout, '');
  assert.equal(result.stderr.trim(), 'Claude review Stop hook could not validate its input.');
});

test('malformed events cannot silently bypass the correction limit or expose input', t => {
  for (const input of ['PRIVATE_SENTINEL', 'null', '{}',
    JSON.stringify({ hook_event_name: 'Stop', stop_hook_active: 'false', last_assistant_message: 'PRIVATE_SENTINEL' }),
    JSON.stringify({ hook_event_name: 'SubagentStop', stop_hook_active: false, last_assistant_message: 'PRIVATE_SENTINEL' }),
    JSON.stringify({ hook_event_name: 'Stop', stop_hook_active: false, last_assistant_message: {} })]) {
    const result = runHook(t, '', false, {}, input);
    assert.equal(result.status, 1);
    assert.equal(result.stdout, '');
    assert.equal(result.stderr.trim(), 'Claude review Stop hook could not validate its input.');
  }
});
