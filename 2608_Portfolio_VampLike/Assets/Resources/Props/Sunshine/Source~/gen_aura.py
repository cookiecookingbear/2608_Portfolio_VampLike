import os, math
from common import write_png, preview, ellipse_ring

# Garlic aura, Lv1: outer ellipse covers 2 units on x (37px at PPU 18); canvas 43 wide for headroom.
OA, OB = 18, 10                      # outer semi-axes -> 37 x 21 px, 1.8:1 like the lightning ripple
IA, IB = 14, 8                       # inner ring, same outer/inner gap feel as LightningRipple_v1_1
W, H = 2 * OA + 7, 2 * OB + 5        # padding keeps the glints/rim inside; pivot stays the exact centre
CX, CY = W // 2, H // 2
OUT = os.path.dirname(os.path.abspath(__file__))

# warmer than the lightning palette: same white core, but sunlit amber instead of cool gold
CORE  = (255, 255, 255, 255)
PALE  = (255, 252, 226, 255)
WARM  = (255, 238, 160, 255)
AMBER = (247, 205,  98, 255)
DEEP  = (226, 170,  62, 255)
CLEAR = (0, 0, 0, 0)

px = [[CLEAR] * W for _ in range(H)]
def put(x, y, c):
    if 0 <= CX + x < W and 0 <= CY + y < H:
        px[CY + y][CX + x] = c
def get(x, y):
    return px[CY + y][CX + x] if (0 <= CX + x < W and 0 <= CY + y < H) else None

# inner ring: quieter, reads as the second contour of the same pool of light
for (x, y) in ellipse_ring(IA, IB):
    put(x, y, AMBER)

# outer ring: the actual Lv1 damage boundary, so it is the brightest line
outer = ellipse_ring(OA, OB)
for (x, y) in outer:
    if y >= 6 and get(x, y + 1) == CLEAR:
        put(x, y + 1, DEEP)                       # ground shadow rim under the near arc
for (x, y) in outer:
    put(x, y, PALE if y > 2 else WARM)

# glints at the four extremes plus the upper-right quadrant, where the sunlight comes from
for (x, y) in ((0, OB), (-1, OB), (1, OB), (-OA, 0), (OA, 0), (0, -OB), (1, -OB), (-1, -OB)):
    put(x, y, CORE)
for (x, y) in ((12, -7), (13, -6), (15, -5), (16, -3)):
    put(x, y, CORE)                               # sun-facing arc catches the light

# a few warm motes between the rings so the band does not read as an empty gap
for (x, y) in ((-16, 4), (-9, -9), (-3, 9), (6, -9), (11, 7), (16, 2), (-13, -6), (3, 9)):
    if get(x, y) == CLEAR:
        put(x, y, AMBER)

write_png(os.path.join(OUT, 'GarlicAura_v1.png'), px)
write_png(os.path.join(OUT, 'GarlicAura_v1_preview_x8.png'), preview(px), scale=8)
print('aura', W, H)
