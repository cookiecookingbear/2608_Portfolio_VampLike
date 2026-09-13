import zlib, struct, os

W, H = 20, 56
OUT = os.path.dirname(os.path.abspath(__file__))

CORE  = (255, 255, 255, 255)
PALE  = (255, 250, 214, 255)
YEL   = (255, 232, 140, 255)
EDGE  = (236, 192,  80, 255)
CLEAR = (0, 0, 0, 0)

# ---- spine: angular zigzag made of near-vertical runs joined by horizontal jogs ----
# (y_start, y_end_exclusive, x_start, x_end)  x drifts linearly inside a run
runs = [
    (0,  5,  10, 10),
    (5,  11, 14, 13),
    (11, 19, 5,  6),
    (19, 27, 14, 13),
    (27, 34, 4,  5),
    (34, 42, 13, 12),
    (42, 50, 7,  8),
]
spine = set()
prev_end = None
for (y0, y1, xa, xb) in runs:
    if prev_end is not None:                      # horizontal jog on the boundary row
        lo, hi = sorted((prev_end, xa))
        for x in range(lo, hi + 1):
            spine.add((x, y0))
    n = y1 - y0
    for i, y in enumerate(range(y0, y1)):
        x = round(xa + (xb - xa) * i / max(n - 1, 1))
        spine.add((x, y))
    prev_end = xb

# thickness profile by y: 0 = 1px, 1 = 3px, 2 = 5px
def radius(y):
    if y < 3:  return 0
    if y < 9:  return 1
    if y < 42: return 2
    if y < 48: return 1
    return 0

def dilate(cells, r, diag=False):
    out = set()
    for (x, y) in cells:
        for dx in range(-r, r + 1):
            for dy in range(-r, r + 1):
                if diag or abs(dx) + abs(dy) <= r:
                    out.add((x + dx, y + dy))
    return out

layers = {}
for (x, y) in spine:
    r = radius(y)                                     # 0:1px 1:3px 2:5px
    pale = dilate({(x, y)}, 1) if r >= 2 else {(x, y)}
    yel  = dilate({(x, y)}, r) if r else {(x, y)}
    for c in dilate(yel, 1, diag=True) - dilate(yel, 1):
        layers.setdefault(c, 2)                       # edge on diagonal corners only
    for c in yel:
        layers[c] = max(layers.get(c, 0), 3)
    for c in pale:
        layers[c] = max(layers.get(c, 0), 4)
    layers[(x, y)] = 5

# ---- branches (1px, yellow with pale tip, edge outline) ----
branches = [
    [(16, 7), (17, 8), (18, 8), (19, 10)],
    [(3, 14), (2, 15), (1, 15), (0, 17)],
    [(16, 22), (17, 23), (18, 23), (19, 25)],
    [(2, 30), (1, 31), (0, 33)],
    [(15, 37), (16, 38), (17, 40)],
]
for b in branches:
    for i, c in enumerate(b):
        layers[c] = max(layers.get(c, 0), 4 if i == len(b) - 1 else 3)
        for n in dilate({c}, 1):
            layers.setdefault(n, 2)

# ---- impact flash at bottom, centred on the spine end ----
ix, iy = 8, 52
for dx in range(-6, 7):
    for dy in range(-3, 4):
        d = abs(dx) + abs(dy) * 2
        if d <= 7: layers[(ix+dx, iy+dy)] = max(layers.get((ix+dx, iy+dy), 0), 2)
        if d <= 5: layers[(ix+dx, iy+dy)] = max(layers.get((ix+dx, iy+dy), 0), 3)
        if d <= 3: layers[(ix+dx, iy+dy)] = max(layers.get((ix+dx, iy+dy), 0), 4)
        if d <= 1: layers[(ix+dx, iy+dy)] = 5
# sparks
for c in ((1, 49), (16, 48), (2, 55), (15, 55), (18, 52)):
    layers.setdefault(c, 3)

COL = {2: EDGE, 3: YEL, 4: PALE, 5: CORE}
px = [[CLEAR] * W for _ in range(H)]
for (x, y), v in layers.items():
    if 0 <= x < W and 0 <= y < H:
        px[y][x] = COL[v]

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

write_png(os.path.join(OUT, 'Lightning.png'), px)
prev = [[(c if c[3] else (70, 70, 80, 255)) for c in row] for row in px]
write_png(os.path.join(OUT, 'Lightning_preview_x8.png'), prev, scale=8)
print('ok')
