import assert from 'node:assert/strict';
import { spawnSync } from 'node:child_process';
import { chmodSync, copyFileSync, mkdirSync, mkdtempSync, readFileSync, rmSync, writeFileSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import test from 'node:test';

const workflow = readFileSync(new URL('../workflows/claude-code-review.yml', import.meta.url), 'utf8').replaceAll('\r\n', '\n');
const installStep = workflow.split('      - name: Install pinned Claude Code\n')[1]?.split('      - name: Run Claude Code Review')[0];
const script = installStep?.split('        run: |\n')[1]?.replace(/^ {10}/gm, '');
assert.ok(script, 'The workflow must install and verify Claude before the review action');
const manifest = JSON.parse(readFileSync(new URL('../claude-code/package.json', import.meta.url), 'utf8'));
const version = manifest.dependencies['@anthropic-ai/claude-code'];
const bash = process.platform === 'win32' ? join(process.env.ProgramFiles, 'Git/bin/bash.exe') : 'bash';

function install(t, environment = {}) {
  const directory = mkdtempSync(join(tmpdir(), 'claude-install-'));
  t.after(() => rmSync(directory, { recursive: true, force: true }));
  const trusted = join(directory, '.github/claude-code');
  mkdirSync(trusted, { recursive: true });
  for (const name of ['package.json', 'package-lock.json']) {
    copyFileSync(new URL(`../claude-code/${name}`, import.meta.url), join(trusted, name));
  }
  mkdirSync(join(directory, 'pr-head'), { recursive: true });
  writeFileSync(join(directory, 'pr-head/package.json'), 'untrusted manifest must never reach npm');
  const bin = join(directory, 'bin');
  mkdirSync(bin);
  writeFileSync(join(bin, 'npm'), `#!/usr/bin/env bash
set -euo pipefail
if [[ "$INSTALL_STATUS" != 0 ]]; then
  echo 'npm ERR! HTTP 429 from registry.npmjs.org' >&2
  exit "$INSTALL_STATUS"
fi
[[ "$1" == ci && "$2" == --prefix ]]
prefix="$3"
cmp .github/claude-code/package.json "$prefix/package.json"
cmp .github/claude-code/package-lock.json "$prefix/package-lock.json"
mkdir -p "$prefix/node_modules/.bin"
cat > "$prefix/node_modules/.bin/claude" <<'CLAUDE'
#!/usr/bin/env bash
printf '%s (Claude Code)\\n' "$CLAUDE_VERSION"
exit "$CLAUDE_STATUS"
CLAUDE
chmod +x "$prefix/node_modules/.bin/claude"
`);
  chmodSync(join(bin, 'npm'), 0o755);
  const output = join(directory, 'output');
  writeFileSync(output, '');
  const result = spawnSync(bash, ['--noprofile', '--norc', '-c', script], {
    cwd: directory,
    encoding: 'utf8',
    env: {
      ...process.env,
      PATH: `${bin.replaceAll('\\', '/')}${process.platform === 'win32' ? ';' : ':'}${process.env.PATH}`,
      RUNNER_TEMP: directory.replaceAll('\\', '/'),
      GITHUB_OUTPUT: output.replaceAll('\\', '/'),
      CLAUDE_VERSION: version,
      INSTALL_STATUS: '0',
      CLAUDE_STATUS: '0',
      ...environment,
    },
    timeout: 30_000,
  });
  assert.ifError(result.error);
  return { ...result, output: readFileSync(output, 'utf8') };
}

test('installation publishes an executable only after trusted manifests and the pinned version are verified', t => {
  const result = install(t);
  assert.equal(result.status, 0, result.stderr);
  assert.match(result.output, /^executable=.+\/claude-code\.[^/]+\/node_modules\/\.bin\/claude\n$/);
  assert.match(result.stdout, /Verified Claude Code/);
});

test('npm download failure keeps its diagnostics and cannot publish an executable', t => {
  const result = install(t, { INSTALL_STATUS: '22' });
  assert.equal(result.status, 22, result.stderr);
  assert.match(result.stderr, /HTTP 429 from registry\.npmjs\.org/);
  assert.equal(result.output, '');
});

test('a different executable version cannot reach the review action', t => {
  const result = install(t, { CLAUDE_VERSION: '0.0.0' });
  assert.equal(result.status, 1, result.stderr);
  assert.match(result.stdout, /::error::Claude Code executable did not report the pinned version/);
  assert.equal(result.output, '');
});

test('an executable that cannot start cannot reach the review action', t => {
  const result = install(t, { CLAUDE_STATUS: '126' });
  assert.equal(result.status, 126, result.stderr);
  assert.equal(result.output, '');
});
