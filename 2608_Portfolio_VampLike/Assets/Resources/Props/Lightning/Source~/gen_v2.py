import zlib, struct, os, random, sys

W, H = 14, 67
OUT = os.path.dirname(os.path.abspath(__file__))

CORE  = (255, 255, 255, 255)
PALE  = (255, 250, 214, 255)
YEL   = (255, 232, 140, 255)
GOLD  = (236, 192,  80, 255)
CLEAR = (0, 0, 0, 0)

def line(a, b):
    """Bresenham cells from a to b inclusive."""
    (x0, y0), (x1, y1) = a, b
    cells = []
    dx, dy = abs(x1 - x0), -abs(y1 - y0)
    sx, sy = (1 if x0 < x1 else -1), (1 if y0 < y1 else -1)
    err = dx + dy
    while True:
        cells.append((x0, y0))
        if (x0, y0) == (x1, y1): break
        e2 = 2 * err
        if e2 >= dy: err += dy; x0 += sx
        if e2 <= dx: err += dx; y0 += sy
    return cells

def build(seed):
    rng = random.Random(seed)
    layers = {}          # cell -> priority (higher wins): 1 gold, 2 yellow, 3 pale, 4 white
    def put(c, v):
        layers[c] = max(layers.get(c, 0), v)

    # ---- trunk: random walk downward, biased back toward the centre ----
    trunk = []
    x, y = rng.randint(5, 8), 0
    end_y = H - 8                      # impact zone starts here
    while y < end_y:
        step = rng.choice([2, 3, 3, 4, 5, 6])
        ny = min(y + step, end_y)
        bias = (W // 2 - x) * 0.35
        dx = rng.choice([-4, -3, -2, -1, 0, 1, 2, 3, 4]) + bias
        nx = max(1, min(W - 2, round(x + dx)))
        # sometimes a flat horizontal jog before continuing down
        if rng.random() < 0.3 and abs(nx - x) >= 2:
            trunk.append(((x, y), (nx, y)))
            trunk.append(((nx, y), (nx, ny)))
        else:
            trunk.append(((x, y), (nx, ny)))
        x, y = nx, ny
    trunk_cells = []
    for a, b in trunk:
        trunk_cells += line(a, b)
    for c in trunk_cells:
        put(c, 4 if 3 <= c[1] <= H - 10 else 3)
    # brief pale "halo" pixels on the outside of sharp corners, keeps it thin elsewhere

    # ---- branches: random origin/direction/length, may fork once ----
    def branch(origin, depth, length, dirx):
        """1-2 straight segments with a kink; sub-branch may sprout at the kink."""
        cx, cy = origin
        pts = [(cx, cy)]
        segs = 1 if length <= 3 or rng.random() < 0.35 else 2
        remaining = length
        for s in range(segs):
            seg_len = remaining if s == segs - 1 else rng.randint(1, remaining - 1)
            remaining -= seg_len
            vx = dirx * rng.choice([1, 1, 2, 2, 3])
            vy = rng.choice([-2, -1, 1, 1, 2, 2, 3, 3, 4])   # mostly downward-outward
            if s == 1 and rng.random() < 0.5:          # kink: bend the other way vertically
                vy = -vy if vy else rng.choice([-1, 1])
            nx = max(0, min(W - 1, cx + vx * seg_len // max(abs(vx), 1) * (1 if abs(vx) >= abs(vy) else 1)))
            # scale so the longer axis advances seg_len pixels
            m = max(abs(vx), abs(vy), 1)
            nx = max(0, min(W - 1, cx + round(vx / m * seg_len)))
            ny = max(0, min(H - 7, cy + round(vy / m * seg_len)))
            pts.append((nx, ny))
            cx, cy = nx, ny
        cells = []
        for a, b in zip(pts, pts[1:]):
            cells += line(a, b)
        for i, c in enumerate(cells):
            put(c, (3 if i == 0 else 2) if depth == 0 else 1)
        if depth == 0 and length >= 4 and rng.random() < 0.6:
            branch(pts[1], 1, rng.randint(2, 3), rng.choice([-1, 1]))
        return pts

    n_br = rng.randint(6, 9)
    corners = [a for a, b in trunk if 4 <= a[1] <= end_y - 6]
    origins = rng.sample(corners, min(len(corners), n_br - 2))
    origins += rng.sample(trunk_cells[4:-6], n_br - len(origins))
    for o in origins:
        dirx = -1 if o[0] >= W // 2 else 1
        if rng.random() < 0.25: dirx = -dirx
        branch(o, 0, rng.randint(3, 9), dirx)

    # ---- impact flash: small flattened burst + scattered sparks ----
    ix, iy = x, H - 4
    put((ix, iy), 4)
    for dx in range(-3, 4):
        put((ix + dx, iy), 3 if abs(dx) <= 1 else 2)
    for dx, dy in ((-2, -1), (2, -1), (-2, 1), (2, 1), (0, -2), (0, 2), (-4, 0), (4, 0)):
        put((ix + dx, iy + dy), 2)
    for dx, dy in ((-5, -2), (5, -2), (-4, 2), (4, 2), (-6, 0), (6, 0)):
        put((ix + dx, iy + dy), 1)
    # connect trunk end to the impact point
    for c in line((x, end_y), (ix, iy - 2)):
        put(c, 4)

    COL = {1: GOLD, 2: YEL, 3: PALE, 4: CORE}
    px = [[CLEAR] * W for _ in range(H)]
    for (cx, cy), v in layers.items():
        if 0 <= cx < W and 0 <= cy < H:
            px[cy][cx] = COL[v]
    return px

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

if __name__ == '__main__':
    if sys.argv[1] == 'sheet':
        seeds = [int(s) for s in sys.argv[2:]]
        gap = 4
        sheet = [[(70, 70, 80, 255)] * (len(seeds) * (W + gap)) for _ in range(H)]
        for i, s in enumerate(seeds):
            px = build(s)
            for y in range(H):
                for xx in range(W):
                    if px[y][xx][3]: sheet[y][i * (W + gap) + xx] = px[y][xx]
        write_png(os.path.join(OUT, 'candidates.png'), sheet, scale=6)
    else:
        seed = int(sys.argv[1]); name = sys.argv[2]
        px = build(seed)
        write_png(os.path.join(OUT, name + '.png'), px)
        prev = [[(c if c[3] else (70, 70, 80, 255)) for c in row] for row in px]
        write_png(os.path.join(OUT, name + '_preview_x8.png'), prev, scale=8)
    print('ok')
