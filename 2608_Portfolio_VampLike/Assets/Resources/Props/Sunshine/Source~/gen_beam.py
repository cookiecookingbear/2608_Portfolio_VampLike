import os, math
from common import write_png, preview

# Sunlight shaft standing over the garlic footprint, cast from the upper right.
# The shape is the aura ellipse swept along the light direction, with the ellipse itself
# cut out (that area is the pool in GarlicAura_v2), so the shaft visually lands on the pool.
OA, OB = 18, 10                       # must match gen_aura.py
UX, UY = 1 / math.sqrt(5), -2 / math.sqrt(5)   # travel direction: 1 right per 2 up
RISE = 160                            # px of vertical rise; the camera only shows 90px above the player,
                                      # so the shaft stays full strength to the screen edge and fades off-screen
TMAX = RISE / -UY
FADE = 50                             # the last stretch fades out so the sprite has no hard cut
SUN = (255, 246, 198)
PEAK = 64                             # 0.25 alpha at the brightest pixel
CLEAR = (0, 0, 0, 0)
OUT = os.path.dirname(os.path.abspath(__file__))

def footprint_hit(x, y):
    """Walk back along the light direction; return (t, signed lateral r) at closest approach."""
    # f(t) = ((x-t*UX)/OA)^2 + ((y-t*UY)/OB)^2 ; minimise over t >= 0
    a = (UX / OA) ** 2 + (UY / OB) ** 2
    b = -2 * (x * UX / OA ** 2 + y * UY / OB ** 2)
    c = (x / OA) ** 2 + (y / OB) ** 2
    t = max(0.0, -b / (2 * a))
    r = math.sqrt(max(0.0, a * t * t + b * t + c))
    qx, qy = x - t * UX, y - t * UY
    side = 1 if (UX * qy - UY * qx) >= 0 else -1
    return t, r * side

def alpha_at(x, y):
    if (x / OA) ** 2 + (y / OB) ** 2 <= 1.0:
        return 0                                   # the pool belongs to GarlicAura_v2
    t, s = footprint_hit(x, y)
    r = abs(s)
    if r > 1.0 or t > TMAX:
        return 0
    rail = 1.0 if r > 0.88 else 0.52               # brighter edges read as a volumetric shaft
    band = 1.0 if int((s + 1) * 3.0) % 2 == 0 else 0.55   # streaks running along the shaft
    far = 1.0 if t < TMAX - FADE else max(0.0, (TMAX - t) / FADE)
    a = PEAK * rail * band * far
    return round(a / PEAK * 8) * (PEAK / 8)        # quantise into steps, like the pool

# measure the shape, then crop to it with a 1px margin
pts = [(x, y) for y in range(-int(RISE) - 4, 24) for x in range(-OA - 4, OA + int(TMAX * UX) + 6)
       if alpha_at(x, y) >= 8]
x0, x1 = min(p[0] for p in pts) - 1, max(p[0] for p in pts) + 1
y0, y1 = min(p[1] for p in pts) - 1, max(p[1] for p in pts) + 1
W, H = x1 - x0 + 1, y1 - y0 + 1
px = [[CLEAR] * W for _ in range(H)]
for (x, y) in pts:
    px[y - y0][x - x0] = SUN + (round(alpha_at(x, y)),)

write_png(os.path.join(OUT, 'GarlicSunbeam_v1.png'), px)
write_png(os.path.join(OUT, 'GarlicSunbeam_v1_preview_grass_x4.png'), preview(px, bg=(58, 140, 90, 255)), scale=4)

# the ellipse centre sits at (-x0, -y0) in pixels from the top-left of this canvas
pivot_x = (-x0 + 0.5) / W
pivot_y = 1 - (-y0 + 0.5) / H          # Unity pivots are measured from the bottom
print('beam %dx%d  custom pivot = (%.4f, %.4f)' % (W, H, pivot_x, pivot_y))
