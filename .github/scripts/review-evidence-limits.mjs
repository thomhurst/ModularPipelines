import { appendFileSync, readFileSync } from 'node:fs';

const changedFiles = readFileSync(process.argv[2], 'utf8').split('\0').filter(Boolean);
const maximum = new Set(changedFiles).size;
appendFileSync(process.env.GITHUB_OUTPUT,
  `minimum_evidence_count=${maximum > 0 ? 1 : 0}\nmaximum_evidence_count=${maximum}\n`);
