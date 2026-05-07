import sys
from pathlib import Path
from pdfminer.high_level import extract_text

if len(sys.argv) < 3:
    print("Usage: extract_pdf.py <src.pdf> <out.txt>")
    sys.exit(2)

src = sys.argv[1]
out = sys.argv[2]
text = extract_text(src)
Path(out).write_text(text, encoding='utf-8')
print(f"WROTE {out}")
