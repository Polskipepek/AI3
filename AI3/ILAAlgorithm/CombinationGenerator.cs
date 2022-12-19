using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI3.ILAAlgorithm {
    public class CombinationGenerator<T> {
        private readonly List<T> _items;
        private readonly int _size;
        private readonly int[] _indices;

        public CombinationGenerator(List<T> items, int size) {
            _items = items;
            _size = size;
            _indices = new int[size];
            for (int i = 0; i < size; i++) {
                _indices[i] = i;
            }
        }

        public IEnumerable<IEnumerable<T>> GetCombinations() {
            yield return _indices.Select(i => _items[i]);

            while (MoveNext()) {
                yield return _indices.Select(i => _items[i]);
            }
        }

        private bool MoveNext() {
            int i;
            for (i = _size - 1; i >= 0 && _indices[i] == _items.Count - _size + i; i--) {
            }

            if (i < 0) {
                return false;
            }

            _indices[i]++;
            for (i++; i < _size; i++) {
                _indices[i] = _indices[i - 1] + 1;
            }

            return true;
        }
    }
}
