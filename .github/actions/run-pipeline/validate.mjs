import { DefaultArtifactClient } from '@actions/artifact';
import { randomBytes } from 'node:crypto';
import { mkdir, mkdtemp, open, rm } from 'node:fs/promises';
import os from 'node:os';
import path from 'node:path';
import { transfer } from './artifact.mjs';

export async function validate() {
  const client = new DefaultArtifactClient();
  const root = await mkdtemp(path.join(os.tmpdir(), 'pipeline-storage-check-'));
  const ids = Array.from({ length: 3 }, (_, index) =>
    `${process.env.GITHUB_RUN_ID}-${process.env.GITHUB_RUN_ATTEMPT}-capacity-${index}`);
  try {
    // Three simultaneous artifacts exceed the Redis memory usage observed in #5649.
    // Keep memory bounded while testing the size of real build-output archives.
    const publications = await Promise.allSettled(ids.map(async id => {
      const producer = path.join(root, id);
      await mkdir(producer);
      const file = await open(path.join(producer, '_build-output.tar.gz'), 'wx');
      try {
        const block = randomBytes(1024 * 1024);
        for (let index = 0; index < 160; index++) {
          await file.write(block);
        }
      } finally {
        await file.close();
      }
      await transfer(client, 'upload', id, producer);
    }));
    if (publications.some(result => result.status === 'rejected')) {
      throw new Error('Concurrent publication failed.');
    }
    const downloads = await Promise.allSettled(ids.map(async id => {
      const consumer = path.join(root, `download-${id}`);
      await mkdir(consumer);
      await transfer(client, 'download', id, consumer);
    }));
    if (downloads.some(result => result.status === 'rejected')) {
      throw new Error('Concurrent download failed.');
    }
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
