import zlib, struct, math

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

def preview(pix, bg=(70, 70, 80, 255)):
    """composite onto an opaque background so alpha is visible in a flat viewer"""
    out = []
    for row in pix:
        o = []
        for (r, g, b, a) in row:
            t = a / 255
            o.append((round(r*t + bg[0]*(1-t)), round(g*t + bg[1]*(1-t)), round(b*t + bg[2]*(1-t)), 255))
        out.append(o)
    return out

def ellipse_ring(a, b):
    cells = set()
    for x in range(-a, a + 1):
        y = b * math.sqrt(max(0.0, 1 - (x / a) ** 2))
        cells.add((x, round(y))); cells.add((x, -round(y)))
    for y in range(-b, b + 1):
        x = a * math.sqrt(max(0.0, 1 - (y / b) ** 2))
        cells.add((round(x), y)); cells.add((-round(x), y))
    return cells
