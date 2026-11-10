import assert from 'node:assert/strict';
import { spawnSync } from 'node:child_process';
import { readFile, stat, writeFile } from 'node:fs/promises';
import path from 'node:path';
import test from 'node:test';
import { fileURLToPath } from 'node:url';
import { ArtifactNotFoundError } from '@actions/artifact';
import { ArtifactValidationError, artifactErrorMessage } from './artifact-errors.mjs';
import { validate } from './validate.mjs';

const secret = 'https://storage.invalid/archive?sig=do-not-log';
const archiveName = '_build-output.tar.gz';

function fixture(t) {
  const artifacts = new Map();
  const producers = [];
  const deleted = [];
  const messages = [];
  const warnings = [];
  t.mock.method(console, 'log', message => messages.push(message));
  t.mock.method(console, 'warn', message => warnings.push(message));
  const client = {
    async uploadArtifact(name, files) {
      artifacts.set(name, await Promise.all(files.map(file => readFile(file))));
      return { id: name };
    },
    async getArtifact(name) {
      assert.equal(artifacts.size, 3, 'All publications must finish before download');
      return { artifact: { id: name, digest: 'expected-digest' } };
    },
    async downloadArtifact(id, options) {
      assert.equal(options.expectedHash, 'expected-digest');
      const [archive, manifest] = artifacts.get(id);
      await writeFile(path.join(options.path, archiveName), archive);
      await writeFile(path.join(options.path, `${archiveName}.json`), manifest);
      return {};
    },
    async deleteArtifact(name) {
      deleted.push(name);
    },
  };
  async function createProbe(producer) {
    producers.push(producer);
    await writeFile(path.join(producer, archiveName), 'probe output');
  }
  return { client, createProbe, producers, deleted, messages, warnings };
}

async function assertCleaned({ producers, deleted }) {
  assert.equal(producers.length, 3);
  assert.equal(deleted.length, 3);
  await assert.rejects(stat(path.dirname(producers[0])), { code: 'ENOENT' });
}

test('concurrent validation waits for publication visibility before verifying all probes', async t => {
  const state = fixture(t);
  const lookup = state.client.getArtifact;
  const calls = new Map();
  state.client.getArtifact = async name => {
    const count = (calls.get(name) ?? 0) + 1;
    calls.set(name, count);
    if (count === 1) throw new ArtifactNotFoundError(secret);
    return lookup(name);
  };
  await validate(state);
  assert.deepEqual([...calls.values()], [2, 2, 2]);
  assert.equal(state.messages.filter(message => message.includes('Waiting for')).length, 3);
  assert.ok(state.messages.some(message => message.startsWith('Validated three concurrent')));
  assert.equal(state.warnings.length, 0);
  await assertCleaned(state);
});

for (const stage of ['preparation', 'publication', 'lookup', 'download', 'digest', 'archive verification']) {
  test(`concurrent validation reports safe ${stage} failures and cleans every probe`, async t => {
    const state = fixture(t);
    const fail = async () => { throw new Error(secret); };
    if (stage === 'preparation') {
      const create = state.createProbe;
      state.createProbe = async producer => { await create(producer); await fail(); };
    }
    if (stage === 'publication') state.client.uploadArtifact = fail;
    if (stage === 'lookup') state.client.getArtifact = fail;
    if (stage === 'download') state.client.downloadArtifact = fail;
    if (stage === 'digest') state.client.downloadArtifact = async () => ({ digestMismatch: true });
    if (stage === 'archive verification') state.client.downloadArtifact = async () => ({});
    await assert.rejects(validate(state), error => {
      const message = artifactErrorMessage(error);
      assert.ok(error instanceof ArtifactValidationError);
      assert.match(message, new RegExp(stage === 'digest' ? 'digest mismatch' : stage));
      for (let index = 0; index < 3; index++) assert.ok(message.includes(`probe ${index}:`));
      assert.equal(message.includes(secret), false);
      assert.equal(state.messages.some(message => message.startsWith('Validated')), false);
      return true;
    });
    await assertCleaned(state);
  });
}

test('cleanup failure warns safely without hiding successful verification', async t => {
  const state = fixture(t);
  const remove = state.client.deleteArtifact;
  state.client.deleteArtifact = async name => {
    await remove(name);
    throw new Error(secret);
  };
  await validate(state);
  assert.deepEqual(state.warnings, ['Some capacity probes could not be deleted; their one-day retention still applies.']);
  await assertCleaned(state);
});

test('safe error formatting never prints untrusted SDK messages or causes', () => {
  assert.equal(artifactErrorMessage(new Error(secret)), 'GitHub build-output artifact transfer failed.');
  assert.equal(artifactErrorMessage(new ArtifactValidationError('Safe stage.', { cause: new Error(secret) })), 'Safe stage.');
});

test('action entry point retains safe failure details and exits unsuccessfully', () => {
  const result = spawnSync(process.execPath, [fileURLToPath(new URL('./run-pipeline.mjs', import.meta.url))], {
    encoding: 'utf8',
    timeout: 30_000,
    env: {
      ...process.env,
      MODULARPIPELINES_ARTIFACT_CHECK: 'true',
      // Fail directory preparation before allocating probes or contacting storage.
      GITHUB_RUN_ID: 'invalid/run', GITHUB_RUN_ATTEMPT: '1',
      ACTIONS_RUNTIME_TOKEN: '', ACTIONS_RESULTS_URL: '',
    },
  });
  assert.equal(result.status, 1, result.stderr);
  assert.match(result.stderr, /Concurrent GitHub artifact validation failed: Concurrent publication failed:/);
  assert.match(result.stderr, /Capacity probe preparation failed/);
  assert.doesNotMatch(result.stderr, /sig=|Error:|at file:/);
});
