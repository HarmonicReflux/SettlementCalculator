import sys
import pathlib

# Put the path “shim” in a single place that pytest loads automatically.
ROOT = pathlib.Path(__file__).resolve().parents[1]
if str(ROOT) not in sys.path:
    sys.path.insert(0, str(ROOT))
