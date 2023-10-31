using System;

public class Heap<T> where T : class, IHeapItem<T> {
	private readonly T[] _items;
	public int Count { get; private set; }

	public Heap(T[] items) {
		_items = items;
	}

	public void Add(T item) {
		item.HeapIndex = Count;
		_items[Count++] = item;
		SortUp(item);
	}

	public T RemoveFirst() {
		T firstItem = _items[0];
		Count--;
		_items[0] = _items[Count];
		_items[0].HeapIndex = 0;
		SortDown(_items[0]);
		return firstItem;
	}

	public bool Contains(T item) {
		return Equals(_items[item.HeapIndex], item);
	}

	public void UpdateItem(T item) {
		SortUp(item);
		SortDown(item);
	}

	private void SortDown(T item) {
		while (true) {
			int childIndexLeft = item.HeapIndex * 2 + 1;
			int childIndexRight = item.HeapIndex * 2 + 2;

			if ( childIndexLeft < Count ) {
				int swapIndex = childIndexLeft;

				if ( childIndexRight < Count ) {
					if ( _items[childIndexLeft].CompareTo(_items[childIndexRight]) < 0 ) {
						swapIndex = childIndexRight;
					}
				}

				if ( item.CompareTo(_items[swapIndex]) < 0 ) {
					Swap(item, _items[swapIndex]);
				} else {
					return;
				}
			} else {
				return;
			}
		}
	}

	private void SortUp(T item) {
		int parentIndex = (item.HeapIndex - 1) / 2;
		while (true) {
			T parentItem = _items[parentIndex];
			if ( item.CompareTo(parentItem) > 0 ) {
				Swap(item, parentItem);
			} else {
				break;
			}

			parentIndex = (item.HeapIndex - 1) / 2;
		}
	}

	private void Swap(T itemA, T itemB) {
		_items[itemA.HeapIndex] = itemB;
		_items[itemB.HeapIndex] = itemA;
		(itemA.HeapIndex, itemB.HeapIndex) = (itemB.HeapIndex, itemA.HeapIndex);
	}

	public void Clear() {
		Array.Clear(_items, 0, Count);
		Count = 0;
	}
}

public interface IHeapItem<T> : IComparable<T> {
	int HeapIndex { get; set; }
}