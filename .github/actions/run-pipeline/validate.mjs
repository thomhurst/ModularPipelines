import { DefaultArtifactClient } from '@actions/artifact';
import { randomBytes } from 'node:crypto';
import { mkdir, mkdtemp, open, rm } from 'node:fs/promises';
import os from 'node:os';
import path from 'node:path';
import { ArtifactValidationError, artifactErrorMessage } from './artifact-errors.mjs';
import { transfer } from './artifact.mjs';

async function createCapacityProbe(producer) {
  // Three simultaneous artifacts exceed the Redis memory usage observed in #5649.
  // Keep memory bounded while testing the size of real build-output archives.
  const file = await open(path.join(producer, '_build-output.tar.gz'), 'wx');
  try {
    const block = randomBytes(1024 * 1024);
    for (let index = 0; index < 160; index++) {
      await file.write(block);
    }
  } finally {
    await file.close();
  }
}

function checkResults(results, stage) {
  const failures = results.flatMap((result, index) => result.status === 'rejected'
    ? [`probe ${index}: ${artifactErrorMessage(result.reason)}`]
    : []);
  if (failures.length) {
    throw new ArtifactValidationError(`Concurrent ${stage} failed: ${failures.join(' ')}`);
  }
}

export async function validate({ client = new DefaultArtifactClient(), createProbe = createCapacityProbe } = {}) {
  const root = await mkdtemp(path.join(os.tmpdir(), 'pipeline-storage-check-'));
  const ids = Array.from({ length: 3 }, (_, index) =>
    `${process.env.GITHUB_RUN_ID}-${process.env.GITHUB_RUN_ATTEMPT}-capacity-${index}`);
  try {
    const publications = await Promise.allSettled(ids.map(async id => {
      const producer = path.join(root, id);
      try {
        await mkdir(producer);
        await createProbe(producer);
      } catch (error) {
        throw new ArtifactValidationError('Capacity probe preparation failed.', { cause: error });
      }
      await transfer(client, 'upload', id, producer);
    }));
    checkResults(publications, 'publication');
    const downloads = await Promise.allSettled(ids.map(async id => {
      const consumer = path.join(root, `download-${id}`);
      await mkdir(consumer);
      // Only these probes have just been successfully published by this process.
      // Ordinary build-output downloads still fail immediately when missing.
      await transfer(client, 'download', id, consumer, { waitForVisibility: true });
    }));
    checkResults(downloads, 'download');
    console.log('Validated three concurrent 160 MiB artifacts (480 MiB total), including size and SHA-256 checks.');
  } finally {
    // These probes are disposable. Real build outputs expire after one day.
    const cleanup = await Promise.allSettled(ids.map(id => client.deleteArtifact(`build-output-${id}`)));
    if (cleanup.some(result => result.status === 'rejected')) {
      console.warn('Some capacity probes could not be deleted; their one-day retention still applies.');
    }
    await rm(root, { recursive: true, force: true });
  }
}
