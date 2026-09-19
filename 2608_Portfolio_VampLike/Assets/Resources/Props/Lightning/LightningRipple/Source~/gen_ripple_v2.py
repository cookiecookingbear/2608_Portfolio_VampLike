import zlib, struct, os, math

W, H = 39, 31
CX, CY = W // 2, H // 2          # pivot = centre pixel; symmetric padding holds the crown above and the rim below
OUT = os.path.dirname(os.path.abspath(__file__))

CORE  = (255, 255, 255, 255)
PALE  = (255, 250, 214, 255)
YEL   = (255, 232, 140, 255)
GOLD  = (236, 192,  80, 255)
CLEAR = (0, 0, 0, 0)

def ellipse_ring(a, b):
    """1px closed ellipse outline, semi-axes a (x) and b (y)."""
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

# inner, trailing ring: dim gold, with gaps so it reads as a fading ripple
for (x, y) in ellipse_ring(11, 5):
    put(x, y, GOLD)

# outer, leading ring: yellow body, pale on the near (bottom) arc, white glints
outer = ellipse_ring(19, 10)
for (x, y) in outer:
    if y >= 6 and px[CY + y + 1][CX + x] == CLEAR:
        put(x, y + 1, GOLD)          # rim under the near arc, sells the tilted view
for (x, y) in outer:
    put(x, y, PALE if y > 3 else YEL)
for (x, y) in ((0, 10), (-1, 10), (1, 10), (-19, 0), (19, 0), (0, -10)):
    put(x, y, CORE)
# crown splash: short spikes rising from the outer ring, each with a detached droplet above
def spike(x, y_base, h, drop):
    lean = (1 if x > 0 else -1) if abs(x) >= 9 else 0   # outer spikes lean outward at the top
    for i in range(1, h + 1):
        put(x + (lean if i > h // 2 else 0), y_base - i, YEL if i < h else PALE)
    put(x + lean, y_base - h - 2, CORE if drop else PALE)
ring_y = {}
for (x, y) in outer:
    ring_y.setdefault(x, []).append(y)
for x, h, drop in ((-16, 2, 0), (-10, 3, 1), (-4, 4, 0), (3, 3, 1), (9, 4, 0), (15, 2, 1)):
    spike(x, min(ring_y[x]), h, drop)            # far (top) arc
for x, h in ((-19, 2), (19, 2)):
    spike(x, min(ring_y[x]), h, 0)               # sides
for x, h, drop in ((-13, 2, 0), (-2, 3, 1), (8, 2, 0), (14, 2, 1)):
    spike(x, max(ring_y[x]), h, drop)            # near (bottom) arc, rises into the ellipse

# small splash pixels just outside the leading ring, like droplets
for (x, y) in ((-6, -12), (7, -12), (-21, -2), (21, -2), (-14, 11), (14, 11)):
    put(x, y, YEL)

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

write_png(os.path.join(OUT, 'LightningRipple_v2.png'), px)
prev = [[(c if c[3] else (70, 70, 80, 255)) for c in row] for row in px]
write_png(os.path.join(OUT, 'LightningRipple_v2_preview_x8.png'), prev, scale=8)
print('ok', W, H)
