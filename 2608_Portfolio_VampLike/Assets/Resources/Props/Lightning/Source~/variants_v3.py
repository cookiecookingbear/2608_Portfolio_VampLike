import sys, os
sys.path.insert(0, os.path.dirname(os.path.abspath(__file__)))
from gen3 import build, write_png

# single-bolt variants: name -> (seed, mid_bias). d/e are right-biased in the middle section.
VARIANTS = {'a': (1, 0), 'b': (6, 0), 'c': (8, 0), 'd': (7, 5), 'e': (3, 5), 'f': (11, 0)}
for k, (seed, mb) in VARIANTS.items():
    px = build(seed, mb)
    write_png('Lightning_v3_%s.png' % k, px)
    prev = [[(c if c[3] else (70, 70, 80, 255)) for c in row] for row in px]
    write_png('Lightning_v3_%s_preview_x8.png' % k, prev, scale=8)
print('ok')
