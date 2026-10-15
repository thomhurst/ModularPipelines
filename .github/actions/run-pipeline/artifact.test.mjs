import assert from 'node:assert/strict';
import { mkdtemp, mkdir, readFile, readdir, rm, writeFile } from 'node:fs/promises';
import os from 'node:os';
import path from 'node:path';
import test from 'node:test';
import { transfer } from './artifact.mjs';

const archiveName = '_build-output.tar.gz';

function memoryClient() {
  const artifacts = new Map();
  return {
    async uploadArtifact(name, files, root, options) {
      assert.equal(options.retentionDays, 1);
      assert.equal(options.compressionLevel, 0);
      assert.equal(artifacts.has(name), false, 'Published artifacts must be immutable');
      const contents = new Map(await Promise.all(files.map(async file => [path.relative(root, file), await readFile(file)])));
      artifacts.set(name, contents);
      return { id: name };
    },
    async getArtifact(name) {
      assert.ok(artifacts.has(name), 'Artifact must be published before download');
      return { artifact: { id: name, digest: 'expected-digest' } };
    },
    async downloadArtifact(id, options) {
      assert.equal(options.expectedHash, 'expected-digest');
      await Promise.all([...artifacts.get(id)].map(([file, data]) => writeFile(path.join(options.path, file), data)));
      return {};
    },
  };
}

async function fixture(t) {
  const root = await mkdtemp(path.join(os.tmpdir(), 'artifact-test-'));
  t.after(() => rm(root, { recursive: true, force: true }));
  const producer = path.join(root, 'producer');
  const consumer = path.join(root, 'consumer');
  await Promise.all([mkdir(producer), mkdir(consumer)]);
  await writeFile(path.join(producer, archiveName), 'complete build output');
  return { producer, consumer, client: memoryClient() };
}

test('overlapping runs and attempts keep independent artifacts during concurrent publication/download', async t => {
  const { producer, client } = await fixture(t);
  const ids = ['123-1', '123-2', '456-1'];
  await Promise.all(ids.map(async id => {
    const repository = path.join(producer, id);
    await mkdir(repository);
    await writeFile(path.join(repository, archiveName), `output for ${id}`);
    await transfer(client, 'upload', id, repository);
    await Promise.all(Array.from({ length: 5 }, async (_, index) => {
      const consumer = path.join(repository, `worker-${index}`);
      await mkdir(consumer);
      await transfer(client, 'download', id, consumer);
      assert.equal(await readFile(path.join(consumer, archiveName), 'utf8'), `output for ${id}`);
      assert.deepEqual(await readdir(consumer), [archiveName]);
    }));
    assert.equal((await readdir(repository)).includes(`${archiveName}.json`), false);
  }));
});

test('dotted distributed run IDs support publication and download', async t => {
  const { producer, consumer, client } = await fixture(t);
  await transfer(client, 'upload', 'Run_1.2-3', producer);
  await transfer(client, 'download', 'Run_1.2-3', consumer);
  assert.equal(await readFile(path.join(consumer, archiveName), 'utf8'), 'complete build output');
});

for (const runId of ['', 'run/id', 'run\\id', 'run:id', 'run id', 'run\nid']) {
  test(`rejects unsafe run ID ${JSON.stringify(runId)} before contacting storage`, async t => {
    const { producer, consumer } = await fixture(t);
    // An empty client makes any storage call fail with a different error.
    for (const [operation, repository] of [['upload', producer], ['download', consumer]]) {
      await assert.rejects(transfer({}, operation, runId, repository), /A safe, attempt-scoped run ID is required/);
    }
    assert.deepEqual(await readdir(producer), [archiveName]);
    assert.deepEqual(await readdir(consumer), []);
  });
}

for (const corruption of ['truncated', 'same-size', 'manifest-missing', 'digest-mismatch', 'download-failure']) {
  test(`rejects ${corruption} without exposing a partial archive`, async t => {
    const { producer, consumer, client } = await fixture(t);
    await transfer(client, 'upload', '123-2', producer);
    const download = client.downloadArtifact;
    client.downloadArtifact = async (id, options) => {
      await download(id, options);
      const archive = path.join(options.path, archiveName);
      if (corruption === 'truncated') await writeFile(archive, 'partial');
      if (corruption === 'same-size') await writeFile(archive, 'x'.repeat('complete build output'.length));
      if (corruption === 'manifest-missing') await rm(`${archive}.json`);
      if (corruption === 'download-failure') throw new Error('connection lost');
      return { digestMismatch: corruption === 'digest-mismatch' };
    };
    await assert.rejects(transfer(client, 'download', '123-2', consumer));
    assert.deepEqual(await readdir(consumer), []);
  });
}

test('failed uploads remove their local manifest and propagate failure', async t => {
  const { producer, client } = await fixture(t);
  client.uploadArtifact = async () => { throw new Error('storage unavailable'); };
  await assert.rejects(transfer(client, 'upload', '123-2', producer), /storage unavailable/);
  assert.deepEqual(await readdir(producer), [archiveName]);
});

test('missing artifacts fail without fallback or hidden retry', async t => {
  const { consumer, client } = await fixture(t);
  await assert.rejects(transfer(client, 'download', '123-2', consumer));
  assert.deepEqual(await readdir(consumer), []);
});
