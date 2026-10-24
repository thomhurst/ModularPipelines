import assert from 'node:assert/strict';
import { mkdtempSync, readFileSync, rmSync, writeFileSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { spawnSync } from 'node:child_process';
import test from 'node:test';
import { runInNewContext } from 'node:vm';

const workflow = readFileSync(new URL('../workflows/dotnet.yml', import.meta.url), 'utf8');
const expression = [...workflow.matchAll(/Publish__IsAlpha:\s*\$\{\{\s*(.*?)\s*\}\}/g)].at(-1)?.[1];
assert.ok(expression, 'The workflow must set Publish__IsAlpha');

test('manual releases default to stable', () => {
  assert.match(workflow, /is-alpha:\r?\n(?: {8}[^\r\n]*\r?\n)*? {8}default: false/);
});

// These boolean operators and property lookups share GitHub Actions and JavaScript semantics.
for (const { name, event, inputs, expected } of [
  { name: 'push without dispatch inputs stays alpha', event: 'push', inputs: {}, expected: true },
  { name: 'pull request without dispatch inputs stays alpha', event: 'pull_request', inputs: {}, expected: true },
  { name: 'validation dispatch with default inputs stays alpha', event: 'workflow_dispatch', inputs: { 'publish-packages': false, 'is-alpha': false }, expected: true },
  { name: 'validation dispatch with explicit alpha stays alpha', event: 'workflow_dispatch', inputs: { 'publish-packages': false, 'is-alpha': true }, expected: true },
  { name: 'publishing dispatch with default alpha input selects stable', event: 'workflow_dispatch', inputs: { 'publish-packages': true, 'is-alpha': false }, expected: false },
  { name: 'publishing dispatch with explicit alpha stays alpha', event: 'workflow_dispatch', inputs: { 'publish-packages': true, 'is-alpha': true }, expected: true },
]) {
  test(name, () => {
    // Hyphenated GitHub Actions property names use bracket lookup in JavaScript.
    const javascript = expression.replace(/inputs\.([a-z-]+)/g, "inputs['$1']");
    assert.equal(runInNewContext(javascript, { github: { event_name: event }, inputs }), expected);
  });
}

test('publishing dispatches accept an explicit expected package version', () => {
  assert.match(workflow, /expected-version:\r?\n(?: {8}[^\r\n]*\r?\n)*? {8}type: string/);
  // Validation dispatches may omit it; the version module requires it when publishing.
  assert.match(workflow, /expected-version:\r?\n(?: {8}[^\r\n]*\r?\n)*? {8}required: false/);
  assert.match(workflow, /Publish__ExpectedVersion:\s*\$\{\{\s*inputs\.expected-version\s*\}\}/);
});

test('prebuild and packing share the publishing policy and compiled version', () => {
  const resolution = workflow.match(/- name: Resolve package version before building([\s\S]*?)- name: Build solutions/)?.[1];
  assert.ok(resolution, 'Resolve the package version before compilation');
  assert.match(resolution, /working-directory: src\/ModularPipelines.Build/);
  assert.match(resolution, /--write-package-version "\$GITHUB_ENV"/);
  assert.equal(resolution.match(/Publish__IsAlpha:\s*\$\{\{\s*(.*?)\s*\}\}/)?.[1], expression);
  for (const setting of ['ShouldPublish', 'ExpectedVersion']) {
    const pattern = new RegExp(`Publish__${setting}:\\s*\\$\\{\\{\\s*(.*?)\\s*\\}\\}`, 'g');
    const values = [...workflow.matchAll(pattern)].map(match => match[1]);
    assert.equal(values.length, 2);
    assert.equal(values[0], values[1]);
  }
});

for (const [version, property] of [['4.2.3', 'VersionPrefix'], ['4.2.3-alpha0012', 'Version']]) {
  test(`solution build passes ${version} through ${property}`, t => {
    const directory = mkdtempSync(join(tmpdir(), 'release-build-'));
    t.after(() => rmSync(directory, { recursive: true, force: true }));
    writeFileSync(join(directory, 'BuildSolutions.txt'), '# fixture\nCore.slnx\nDistributed.slnx\n');
    const step = workflow.match(/- name: Build solutions[\s\S]*?        run: \|\r?\n([\s\S]*?)(?=      - (?:name:|uses:))/)?.[1];
    assert.ok(step, 'The workflow must build solutions');
    const script = step.replace(/^ {10}/gm, '');
    // Capture the actual build arguments without compiling fixture solutions.
    const bash = process.platform === 'win32' ? join(process.env.ProgramFiles, 'Git/bin/bash.exe') : 'bash';
    const result = spawnSync(bash, ['--noprofile', '--norc', '-e', '-o', 'pipefail', '-c',
      'dotnet() { printf "[%s]\\n" "$@"; }\n' + script], {
      cwd: directory,
      env: { ...process.env, Pipeline__PackageVersion: version },
      encoding: 'utf8',
    });
    assert.ifError(result.error);
    assert.equal(result.status, 0, result.stderr);
    assert.ok(result.stdout.includes('[Core.slnx]'));
    assert.ok(result.stdout.includes('[Distributed.slnx]'));
    assert.equal(result.stdout.split(`[-p:${property}=${version}]`).length - 1, 2);
  });
}
