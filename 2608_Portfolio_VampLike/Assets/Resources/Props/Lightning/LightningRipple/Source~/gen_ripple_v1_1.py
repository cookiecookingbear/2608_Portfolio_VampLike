import zlib, struct, os, math

# v1_1 = v1 with the inner ring widened (11/5 -> 14/7 at v1 scale) and everything scaled to 70%
S = 0.7
OA, OB = round(19 * S), round(10 * S)      # outer ring semi-axes -> 13, 7
IA, IB = round(14 * S), round(7 * S)       # inner ring semi-axes -> 10, 5
W, H = 2 * OA + 1, 2 * OB + 3              # +1 rim row below, +1 pad row above keeps the pivot centred
CX, CY = W // 2, H // 2
OUT = os.path.dirname(os.path.abspath(__file__))

CORE  = (255, 255, 255, 255)
PALE  = (255, 250, 214, 255)
YEL   = (255, 232, 140, 255)
GOLD  = (236, 192,  80, 255)
CLEAR = (0, 0, 0, 0)

def ellipse_ring(a, b):
    cells = set()
    for x in range(-a, a + 1):
        y = b * math.sqrt(max(0.0, 1 - (x / a) ** 2))
        cells.add((x, round(y))); cells.add((x, -round(y)))
    for y in range(-b, b + 1):
        x = a * math.sqrt(max(0.0, 1 - (y / b) ** 2))
        cells.add((round(x), y)); cells.add((-round(x), y))
    return cells

px = [[CLEAR] * W for _ in range(H)]
def put(x, y, c):
    if 0 <= CX + x < W and 0 <= CY + y < H:
        px[CY + y][CX + x] = c
def get(x, y):
    return px[CY + y][CX + x] if (0 <= CX + x < W and 0 <= CY + y < H) else None

for (x, y) in ellipse_ring(IA, IB):
    put(x, y, GOLD)                                   # inner, trailing ring
outer = ellipse_ring(OA, OB)
for (x, y) in outer:
    if y >= round(6 * S) and get(x, y + 1) == CLEAR:
        put(x, y + 1, GOLD)                           # rim under the near arc
for (x, y) in outer:
    put(x, y, PALE if y > round(3 * S) else YEL)      # leading ring, brighter on the near arc
for (x, y) in ((0, OB), (-1, OB), (1, OB), (-OA, 0), (OA, 0), (0, -OB)):
    put(x, y, CORE)                                   # glints
for (x, y) in ((-round(14 * S), round(11 * S)), (round(14 * S), round(11 * S))):
    put(x, y, YEL)                                    # droplets below the near arc (the only ones inside the v1 canvas)

def write_png(path, pix, scale=1):
    w, h = len(pix[0]) * scale, len(pix) * scale
    raw = bytearray()
    for y in range(h):
        raw.append(0)
        for x in range(w):
            raw += bytes(pix[y // scale][x // scale])
    def chunk(t, d):
        return struct.pack('>I', len(d)) + t + d + struct.pack('>I', zlib.crc32(t + d) & 0xffffffff)
    data = b'\x89PNG\r\n\x1a\n' + chunk(b'IHDR', struct.pack('>IIBBBBB', w, h, 8, 6, 0, 0, 0))
    data += chunk(b'IDAT', zlib.compress(bytes(raw), 9)) + chunk(b'IEND', b'')
    open(path, 'wb').write(data)

write_png(os.path.join(OUT, 'LightningRipple_v1_1.png'), px)
prev = [[(c if c[3] else (70, 70, 80, 255)) for c in row] for row in px]
write_png(os.path.join(OUT, 'LightningRipple_v1_1_preview_x8.png'), prev, scale=8)
print('ok', W, H, 'outer', OA, OB, 'inner', IA, IB)
