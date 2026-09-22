import os
from common import write_png, preview
import gen_aura as A          # rebuilds the ring sprite as a side effect; A.px holds it
import gen_sun as S           # same canvas and centre

OUT = os.path.dirname(os.path.abspath(__file__))
H, W = len(A.px), len(A.px[0])
assert (H, W) == (len(S.px), len(S.px[0]))

# v2 = the pooled sunlight with the range rings drawn over it, as one sprite for one renderer
px = [[S.px[y][x] if A.px[y][x][3] == 0 else A.px[y][x] for x in range(W)] for y in range(H)]

write_png(os.path.join(OUT, 'GarlicAura_v2.png'), px)
write_png(os.path.join(OUT, 'GarlicAura_v2_preview_x8.png'), preview(px), scale=8)
write_png(os.path.join(OUT, 'GarlicAura_v2_preview_grass_x8.png'), preview(px, bg=(58, 140, 90, 255)), scale=8)
print('aura v2', W, H)
