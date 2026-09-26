import assert from 'node:assert/strict';
import { spawnSync } from 'node:child_process';
import { mkdtempSync, rmSync, writeFileSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import test from 'node:test';
import { fileURLToPath } from 'node:url';
import { classifyFailure } from './report-claude-failure.mjs';

test('failed terminal results classify without returning private details', () => {
  for (const [result, category] of [
    ["You've hit your limit PRIVATE_SENTINEL", 'rate or usage limit'],
    ['rate_limit_error PRIVATE_SENTINEL', 'rate or usage limit'],
    ['Your credit balance is too low PRIVATE_SENTINEL', 'rate or usage limit'],
    ['authentication_error PRIVATE_SENTINEL', 'authentication failure'],
    ['OAuth token has expired PRIVATE_SENTINEL', 'authentication failure'],
    ['overloaded_error PRIVATE_SENTINEL', 'service or connection failure'],
    ['PRIVATE_SENTINEL', 'unclassified model error'],
  ]) {
    assert.equal(classifyFailure(JSON.stringify([
      { type: 'user', content: 'PRIVATE_TRANSCRIPT' },
      { type: 'result', subtype: 'success', is_error: true, result },
    ])), category);
  }
  assert.equal(classifyFailure(JSON.stringify([
    { type: 'result', is_error: true, errors: ['ECONNRESET PRIVATE_SENTINEL'] },
  ])), 'service or connection failure');
});

test('successful reviews and intermediate messages cannot classify an action failure', () => {
  for (const messages of [null, [],
    [{ type: 'assistant', content: 'rate_limit_error PRIVATE_SENTINEL' }],
    [{ type: 'result', is_error: false, result: 'rate_limit_error PRIVATE_SENTINEL' }],
  ]) {
    assert.equal(classifyFailure(JSON.stringify(messages)), 'unclassified failure');
  }
  assert.equal(classifyFailure('PRIVATE_SENTINEL malformed JSON'), 'unavailable execution');
});

test('numeric SDK statuses and legacy API prefixes map only to fixed categories', () => {
  for (const [status, category] of [[400, 'invalid API request'], [401, 'authentication failure'],
    [402, 'billing failure'], [403, 'permission failure'], [404, 'API resource not found'],
    [409, 'API resource conflict'], [413, 'request too large'], [429, 'rate or usage limit'],
    [500, 'service or connection failure'], [502, 'service or connection failure'],
    [503, 'service or connection failure'], [504, 'service or connection failure'],
    [529, 'service or connection failure']]) {
    for (const details of [{ api_error_status: status, result: 'PRIVATE_SENTINEL' },
      { result: `API Error: ${status} PRIVATE_SENTINEL` }]) {
      assert.equal(classifyFailure(JSON.stringify([{ type: 'result', is_error: true, ...details }])), category);
    }
  }
  for (const api_error_status of ['PRIVATE_SENTINEL', '401', 999]) {
    assert.equal(classifyFailure(JSON.stringify([
      { type: 'result', is_error: true, api_error_status, result: 'PRIVATE_SENTINEL' },
    ])), 'unclassified model error');
  }
});

test('command-line diagnostics never print result content or filesystem errors', t => {
  const directory = mkdtempSync(join(tmpdir(), 'review-failure-'));
  t.after(() => rmSync(directory, { recursive: true, force: true }));
  const executionFile = join(directory, 'execution.json');
  writeFileSync(executionFile, JSON.stringify([
    { type: 'result', is_error: true, result: "You've hit your limit PRIVATE_SENTINEL" },
  ]));
  for (const [file, category] of [[executionFile, 'rate or usage limit'],
    [join(directory, 'PRIVATE_SENTINEL-missing.json'), 'unavailable execution']]) {
    const result = spawnSync(process.execPath, [fileURLToPath(new URL('./report-claude-failure.mjs', import.meta.url))], {
      encoding: 'utf8', shell: false,
      env: { ...process.env, REVIEW_EXECUTION_FILE: file },
    });
    assert.equal(result.status, 0);
    assert.equal(result.stderr, '');
    assert.equal(result.stdout.trim(), `Claude execution failure category: ${category}.`);
  }
});
