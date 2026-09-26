import assert from 'node:assert/strict';
import { spawnSync } from 'node:child_process';
import { mkdtempSync, writeFileSync, rmSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import test from 'node:test';
import { fileURLToPath } from 'node:url';
import { classifyFailure, diagnose } from './diagnose-claude-failure.mjs';

test('classifies errors without returning transcript content', () => {
  for (const [message, expected] of [
    ['You have hit your usage limit', 'usage-or-rate-limit'],
    ['OAuth token expired', 'authentication'],
    ['Credit balance too low', 'billing'],
    ['Model not found', 'model-unavailable'],
    ['Service unavailable', 'service-or-network'],
    ['Missing structured_output', 'structured-output'],
    ['Unexpected error', 'unclassified-error'],
  ]) {
    assert.equal(classifyFailure([{ type: 'result', is_error: true, result: `${message} SECRET_CANARY` }]), expected);
  }
  assert.equal(classifyFailure([{ type: 'assistant', isApiErrorMessage: true, message: { content: [{ text: 'rate limit SECRET_CANARY' }] } }]), 'usage-or-rate-limit');
  assert.equal(classifyFailure([{ type: 'result', is_error: false, result: 'OAuth success' }]), 'no-error-record');
});

test('command line prints only a fixed category for private errors and malformed output', t => {
  const root = mkdtempSync(join(tmpdir(), 'claude-diagnostic-cli-'));
  t.after(() => rmSync(root, { recursive: true }));
  const file = join(root, 'claude-execution-output.json');
  for (const [content, expected] of [
    [JSON.stringify([{ type: 'result', is_error: true, result: 'OAuth token expired SECRET_CANARY' }]), 'authentication'],
    [JSON.stringify([{ type: 'result', is_error: true, result: 'SECRET_CANARY' }]), 'unclassified-error'],
    ['SECRET_CANARY', 'output-unavailable'],
  ]) {
    writeFileSync(file, content);
    const result = spawnSync(process.execPath, [fileURLToPath(new URL('./diagnose-claude-failure.mjs', import.meta.url))], {
      encoding: 'utf8', shell: false,
      env: { ...process.env, RUNNER_TEMP: root, CLAUDE_EXECUTION_FILE: file },
    });
    assert.equal(result.status, 0);
    assert.equal(result.stderr, '');
    assert.equal(result.stdout.trim(), `Claude failure category: ${expected}`);
    assert.doesNotMatch(result.stdout + result.stderr, /SECRET_CANARY/);
  }
});

test('supports JSON and JSONL while confining files to the runner temporary directory', () => {
  const root = mkdtempSync(join(tmpdir(), 'claude-diagnostic-'));
  const file = join(root, 'claude-execution-output.json');
  try {
    const error = { type: 'result', is_error: true, result: 'Invalid API key SECRET_CANARY' };
    writeFileSync(file, JSON.stringify([error]));
    assert.equal(diagnose('', root), 'authentication');
    writeFileSync(file, `${JSON.stringify({ type: 'system' })}\n${JSON.stringify(error)}\n`);
    assert.equal(diagnose(file, root), 'authentication');
    assert.equal(diagnose(import.meta.filename, root), 'invalid-output-path');
    writeFileSync(file, 'SECRET_CANARY');
    assert.equal(diagnose(file, root), 'output-unavailable');
  } finally {
    rmSync(root, { recursive: true });
  }
});
