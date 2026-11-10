import assert from 'node:assert/strict';
import { spawnSync } from 'node:child_process';
import { readFileSync } from 'node:fs';
import { join } from 'node:path';
import test from 'node:test';

const workflow = readFileSync(new URL('../workflows/auto-merge-generated-options.yml', import.meta.url), 'utf8')
  .replaceAll('\r\n', '\n');
const resolveJob = workflow.split('  resolve:\n')[1]?.split('  arm:\n')[0];
const script = resolveJob?.split('        run: |\n')[1]?.split('          # Filter before checkout:')[0]
  .replace(/^ {10}/gm, '');
assert.ok(script, 'The resolver must find the PR before checking eligibility and loading trusted scripts');
const bash = process.platform === 'win32' ? join(process.env.ProgramFiles, 'Git/bin/bash.exe') : 'bash';

function resolve(environment = {}) {
  const result = spawnSync(bash, ['--noprofile', '--norc', '-e', '-o', 'pipefail', '-c', `
function gh() {
  printf '<%s>' "$@" >&2
  printf '%s' "$TEST_BRANCH_PR"
}
${script}
printf 'resolved=%s\\n' "$pr"
`], {
    encoding: 'utf8', timeout: 10_000,
    env: { ...process.env, DISPATCH_PR: '', COMMENT_PR: '', RUN_BRANCH: '', RUN_TITLE: '',
      TEST_BRANCH_PR: '7293', ...environment },
  });
  assert.ifError(result.error);
  assert.equal(result.status, 0, result.stderr);
  return result;
}

test('manual Claude completion resolves the reviewed PR while running on main', () => {
  // Both expressions are essential: accepting the title inside the step does not
  // help if the job still filters out completions from main.
  assert.doesNotMatch(resolveJob.split('    runs-on:')[0], /head_branch/);
  assert.match(workflow, /RUN_TITLE: \$\{\{ github\.event\.workflow_run\.display_title \}\}/);
  const claude = readFileSync(new URL('../workflows/claude-code-review.yml', import.meta.url), 'utf8');
  assert.match(claude, /run-name: 'Claude review #\$\{\{ github\.event\.pull_request\.number \|\| inputs\.pr_number \}\}'/);
  const result = resolve({ RUN_BRANCH: 'main', RUN_TITLE: 'Claude review #7293' });
  assert.equal(result.stdout, 'resolved=7293\n');
  assert.equal(result.stderr, '', 'A manual run must not look up a PR whose head is main');
});

test('automatic Claude completions and direct events retain their PR identity', () => {
  for (const environment of [
    { RUN_BRANCH: 'automated/update-cli-options-git', RUN_TITLE: 'Claude review #7293' },
    { DISPATCH_PR: '7293' },
    { COMMENT_PR: '7293' },
  ]) {
    const result = resolve(environment);
    assert.equal(result.stdout, 'resolved=7293\n');
    assert.equal(result.stderr, '');
  }
});

test('legacy generated-branch completions still resolve before the named-run rollout', () => {
  const result = resolve({ RUN_BRANCH: 'automated/update-cli-options-git', RUN_TITLE: 'Previous PR title' });
  assert.equal(result.stdout, 'resolved=7293\n');
  assert.match(result.stderr, /<pr><list><--state><open><--head><automated\/update-cli-options-git>/);
});

test('unidentified and malformed completions never look up main or execute title data', () => {
  for (const title of ['', 'Claude Code Review', 'Claude review #0', 'Claude review #7293 trailing',
    'Claude review #$(printf injected)', 'Claude review #7293\nextra']) {
    const result = resolve({ RUN_BRANCH: 'main', RUN_TITLE: title });
    assert.match(result.stdout, /No open pull request for this event/);
    assert.doesNotMatch(result.stdout, /resolved=|injected/);
    assert.equal(result.stderr, '');
  }
});
