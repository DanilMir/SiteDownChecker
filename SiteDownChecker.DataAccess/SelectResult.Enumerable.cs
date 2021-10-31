using System.Collections;
using System.Collections.Generic;

namespace SiteDownChecker.DataAccess
{
    public readonly partial struct SelectResult : IEnumerable<SelectResultObject>
    {
        public IEnumerator<SelectResultObject> GetEnumerator() => new SelectResultEnumerator(this);
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private struct SelectResultEnumerator : IEnumerator<SelectResultObject>
        {
            private readonly SelectResult _selectResult;

            public SelectResultEnumerator(SelectResult selectResult)
            {
                _selectResult = selectResult;
                _currentIndex = -1;
            }

            private int _currentIndex;
            public bool MoveNext() => ++_currentIndex < _selectResult.Count;

            public void Reset() => _currentIndex = -1;

            public SelectResultObject Current => _selectResult[_currentIndex];

            object IEnumerator.Current => Current;

            public void Dispose() { }
        }
    }
}
