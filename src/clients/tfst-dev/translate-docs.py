#!/usr/bin/env python3
import os
import re
import json
import time
import hashlib
import requests
import argparse
from pathlib import Path

# === CONFIG ===
DOCS_DIR = "docs"
OUTPUT_TEMPLATE = "i18n/{lang}/docusaurus-plugin-content-docs/current"
CACHE_TEMPLATE = "i18n/{lang}/i18n-cache.json"
FRONTMATTER_REGEX = re.compile(r"^---\n(.*?)\n---\n", re.DOTALL)


def load_cache(cache_path: Path) -> dict:
    if cache_path.exists():
        with open(cache_path, "r", encoding="utf-8") as f:
            return json.load(f)
    return {}


def save_cache(cache_path: Path, cache_data: dict):
    with open(cache_path, "w", encoding="utf-8") as f:
        json.dump(cache_data, f, indent=2, ensure_ascii=False)


def ensure_frontmatter(md_path: Path) -> tuple[str, str]:
    content = md_path.read_text(encoding="utf-8")
    match = FRONTMATTER_REGEX.match(content)

    if match:
        return match.group(0), content[match.end():]

    # No frontmatter: generate and prepend
    id = md_path.stem
    title = id.replace("-", " ").replace("_", " ").title()
    frontmatter = f"---\nid: {id}\ntitle: {title}\n---\n\n"
    new_content = frontmatter + content
    md_path.write_text(new_content, encoding="utf-8")
    print(f"🛠️  Added frontmatter to {md_path}")
    return frontmatter, content


def compute_hash(content: str) -> str:
    return hashlib.sha256(content.encode("utf-8")).hexdigest()


def translate_text(text: str, target_lang: str, api_key: str) -> str:
    url = "https://translation.googleapis.com/language/translate/v2"
    params = {
        "q": text,
        "target": target_lang,
        "format": "text",
        "key": api_key
    }
    response = requests.post(url, data=params)
    if response.status_code != 200:
        raise Exception(f"Google Translate API error: {response.text}")
    return response.json()["data"]["translations"][0]["translatedText"]


def translate_markdown_file(md_path: Path, lang: str, api_key: str, cache: dict, output_dir: Path):
    frontmatter, content = ensure_frontmatter(md_path)
    content_hash = compute_hash(content)

    relative_path = md_path.relative_to(DOCS_DIR)
    output_path = output_dir / relative_path

    cached_hash = cache.get(str(relative_path))
    if cached_hash == content_hash and output_path.exists():
        print(f"✅ Skipping (no changes): {relative_path}")
        return

    print(f"🌍 Translating: {relative_path}")
    translated = translate_text(content, lang, api_key)

    output_path.parent.mkdir(parents=True, exist_ok=True)
    output_path.write_text(frontmatter + translated, encoding="utf-8")
    cache[str(relative_path)] = content_hash


def main():
    parser = argparse.ArgumentParser(description="Translate /docs using Google Translate API")
    parser.add_argument("--lang", required=True, help="Target language code (e.g., es, fr, de)")
    parser.add_argument("--key", required=True, help="Google Translate API key")
    args = parser.parse_args()

    output_dir = Path(OUTPUT_TEMPLATE.format(lang=args.lang))
    cache_path = Path(CACHE_TEMPLATE.format(lang=args.lang))
    cache = load_cache(cache_path)

    for md_path in Path(DOCS_DIR).rglob("*.md"):
        translate_markdown_file(md_path, args.lang, args.key, cache, output_dir)
        time.sleep(0.1)  # Be gentle with the API

    save_cache(cache_path, cache)
    print(f"\n🎉 Done. Translations saved to: {output_dir}")

if __name__ == "__main__":
    main()
