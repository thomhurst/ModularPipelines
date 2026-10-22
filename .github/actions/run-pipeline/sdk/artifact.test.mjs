import assert from 'node:assert/strict';
import { mkdtemp, mkdir, readFile, readdir, rm, writeFile } from 'node:fs/promises';
import { createRequire } from 'node:module';
import os from 'node:os';
import path from 'node:path';
import { pipeline } from 'node:stream/promises';
import test from 'node:test';
import { DefaultArtifactClient } from '@actions/artifact';

// Exercise the SDK's installed dependencies, rather than a separately installed ZIP stack.
const sdkEntry = import.meta.resolve('@actions/artifact');
const sdkRequire = createRequire(sdkEntry);
const archiverRequire = createRequire(sdkRequire.resolve('archiver'));
const utils = archiverRequire('archiver-utils');
const unzip = sdkRequire('unzip-stream');
const { createZipUploadStream } = await import(new URL('./internal/upload/zip.js', sdkEntry));
const { getUploadZipSpecification } = await import(new URL('./internal/upload/upload-zip-specification.js', sdkEntry));

async function fixture(t) {
  const root = await mkdtemp(path.join(os.tmpdir(), 'artifact-sdk-'));
  t.after(() => rm(root, { recursive: true, force: true }));
  return root;
}

// archiver-utils 5.0.2 only calls glob.sync; cover that contract across the scoped major override.
test('archiver-utils retains CommonJS glob expansion, exclusions and file filtering', { timeout: 30_000 }, async t => {
  const root = await fixture(t);
  await mkdir(path.join(root, 'nested'));
  await Promise.all(['archive.tar.gz', 'nested/manifest.json', 'nested/skip.json', '.hidden.json'].map(
    file => writeFile(path.join(root, file), file)));

  const matches = utils.file.expand(
    { cwd: root, dot: true, filter: 'isFile' },
    ['**/*', '!**/skip.json', '**/manifest.json']);
  assert.deepEqual(matches.sort(), ['.hidden.json', 'archive.tar.gz', path.join('nested', 'manifest.json')]);
  assert.deepEqual(utils.file.expand({ cwd: root }, 'missing-*'), []);
});

test('installed artifact SDK imports and round-trips the archive and manifest through ZIP', { timeout: 30_000 }, async t => {
  assert.equal(typeof new DefaultArtifactClient().uploadArtifact, 'function');
  const root = await fixture(t);
  const source = path.join(root, 'source');
  const destination = path.join(root, 'destination');
  await Promise.all([mkdir(source), mkdir(destination)]);
  const files = new Map([
    ['_build-output.tar.gz', Buffer.from([0x1f, 0x8b, 0, 255, 1, 2, 3])],
    ['_build-output.tar.gz.json', Buffer.from('{"size":7,"sha256":"test"}')],
  ]);
  await Promise.all([...files].map(([name, contents]) => writeFile(path.join(source, name), contents)));
  const specification = getUploadZipSpecification([...files.keys()].map(name => path.join(source, name)), source);
  const zip = await createZipUploadStream(specification, 0);
  const extraction = unzip.Extract({ path: destination });
  // unzip-stream signals completed file writes with close, after its writable finish event.
  const closed = new Promise((resolve, reject) => extraction.once('close', resolve).once('error', reject));
  await Promise.all([pipeline(zip, extraction), closed]);

  assert.deepEqual((await readdir(destination)).sort(), [...files.keys()].sort());
  for (const [name, contents] of files) {
    assert.deepEqual(await readFile(path.join(destination, name)), contents);
  }
});
