import fs from "node:fs";
import path from "node:path";

const root = process.cwd();
const srcDir = path.join(root, "node_modules", "easymde", "dist");
const destDir = path.join(root, "wwwroot", "lib", "easymde");

fs.mkdirSync(destDir, { recursive: true });

const files = ["easymde.min.js", "easymde.min.css"];

for (const file of files) {
  const from = path.join(srcDir, file);
  const to = path.join(destDir, file);

  if (!fs.existsSync(from)) {
    throw new Error(`Missing ${from}. Did easymde install correctly?`);
  }

  fs.copyFileSync(from, to);
  console.log(`Copied ${file} -> ${path.relative(root, to)}`);
}