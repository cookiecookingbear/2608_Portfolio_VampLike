import sys, os
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
from gen3 import build, write_png, W, H, PAD, CORE, CLEAR

# single-bolt variants: name -> (seed, mid_bias). d/e are right-biased in the middle section.
VARIANTS = {'a': (1, 0), 'b': (6, 0), 'c': (8, 0), 'd': (7, 5), 'e': (3, 5), 'f': (11, 0)}
# manual pixel removals after generation (in the 14px-wide generator coordinates): name -> [(x, y), ...]
# b: a branch clamped at the left edge produced a straight 12px vertical yellow line (2026-09-14)
ERASE = {'b': [(0, y) for y in range(40, 52)]}

# output canvas: odd width so the impact point sits exactly on the bottom-centre pivot column.
# 25 = widest offset any variant needs (impact x=12 with trunk content from x=0, or the ±7px flash) on both sides.
OUT_W, OUT_CX = 25, 12

if __name__ == '__main__':
    for k, (seed, mb) in VARIANTS.items():
        px = build(seed, mb)
        for x, y in ERASE.get(k, []):
            px[y][x + PAD] = CLEAR
        ix = next(x for x in range(W + 2 * PAD) if px[H - 1][x] == CORE)   # impact core, bottom row
        dx = OUT_CX - ix
        out = [[CLEAR] * OUT_W for _ in range(H)]
        for y in range(H):
            for x in range(W + 2 * PAD):
                if px[y][x][3]:
                    assert 0 <= x + dx < OUT_W, (k, x + dx)
                    out[y][x + dx] = px[y][x]
        write_png('Lightning_v3_%s.png' % k, out)
        prev = [[(c if c[3] else (70, 70, 80, 255)) for c in row] for row in out]
        write_png('Lightning_v3_%s_preview_x8.png' % k, prev, scale=8)
        print(k, 'impact', ix, '->', OUT_CX, 'shift', dx)
