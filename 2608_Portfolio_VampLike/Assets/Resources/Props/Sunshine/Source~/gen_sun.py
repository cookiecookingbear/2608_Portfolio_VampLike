import os, math
from common import write_png, preview

# Sunlight pooling inside the garlic aura, cast from the upper right.
# Same canvas and centre as GarlicAura_v1 so both renderers can sit at the same transform.
OA, OB = 18, 10
W, H = 2 * OA + 7, 2 * OB + 5
CX, CY = W // 2, H // 2
OUT = os.path.dirname(os.path.abspath(__file__))

SUN  = (255, 246, 198)               # warm sunlight tint
PEAK = 64                            # ~0.25 opacity at the brightest pixel
CLEAR = (0, 0, 0, 0)

px = [[CLEAR] * W for _ in range(H)]

for yy in range(H):
    for xx in range(W):
        x, y = xx - CX, yy - CY
        d = (x / OA) ** 2 + (y / OB) ** 2
        if d > 1.0:
            continue
        # distance from the upper-right rim, normalised: 1 at the sunlit edge, 0 at the far corner
        lit = (x / OA - y / OB) / 2 * 0.5 + 0.5
        # soft falloff toward the ellipse edge so the pool has no hard outline
        edge = min(1.0, (1.0 - d) * 6.0)   # only the last ~1px of the rim softens
        # diagonal beams running with the light direction; hard-edged, not a blur
        phase = ((x + y) % 7 + 7) % 7
        beam = 1.0 if phase < 3 else (0.55 if phase < 5 else 0.3)
        a = PEAK * (0.5 + 0.5 * lit) * edge * beam
        # quantise to a few steps so the shafts read as pixel-art bands
        a = round(a / PEAK * 4) * (PEAK / 4)
        if a >= 8:
            px[yy][xx] = SUN + (round(a),)

write_png(os.path.join(OUT, 'GarlicSunlight_v1.png'), px)
write_png(os.path.join(OUT, 'GarlicSunlight_v1_preview_x8.png'), preview(px), scale=8)
write_png(os.path.join(OUT, 'GarlicSunlight_v1_preview_grass_x8.png'), preview(px, bg=(58, 140, 90, 255)), scale=8)
print('sun', W, H, 'max alpha', max(c[3] for row in px for c in row))
