﻿namespace Sapia.Game.Combat.Pathing;

// https://visualstudiomagazine.com/Articles/2012/11/01/Priority-Queues-with-C.aspx?Page=2
public class PriorityQueue<T> where T : IComparable<T>
{
    public int Count => _data.Count;

    public bool IsEmpty => _data.Count == 0;

    private List<T> _data;

    public PriorityQueue()
    {
        _data = new List<T>();
    }

    public void Clear()
    {
        _data.Clear();
    }

    public void Enqueue(T item)
    {
        _data.Add(item);
        int ci = _data.Count - 1; // child index; start at end
        while (ci > 0)
        {
            int pi = (ci - 1) / 2; // parent index
            if (_data[ci].CompareTo(_data[pi]) >= 0) break; // child item is larger than (or equal) parent so we're done
            (_data[ci], _data[pi]) = (_data[pi], _data[ci]); // Swap
            ci = pi;
        }
    }

    public T Dequeue()
    {
        // assumes pq is not empty; up to calling code
        int li = _data.Count - 1; // last index (before removal)
        T frontItem = _data[0];   // fetch the front
        _data[0] = _data[li];
        _data.RemoveAt(li);

        --li; // last index (after removal)
        int pi = 0; // parent index. start at front of pq
        while (true)
        {
            int ci = pi * 2 + 1; // left child index of parent
            if (ci > li) break;  // no children so done
            int rc = ci + 1;     // right child
            if (rc <= li && _data[rc].CompareTo(_data[ci]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
                ci = rc;
            if (_data[pi].CompareTo(_data[ci]) <= 0) break; // parent is smaller than (or equal to) smallest child so done
            (_data[pi], _data[ci]) = (_data[ci], _data[pi]); // Swap
            pi = ci;
        }
        return frontItem;
    }

    public T Peek()
    {
        T frontItem = _data[0];
        return frontItem;
    }
        
    public override string ToString()
    {
        string s = "";
        for (int i = 0; i < _data.Count; ++i)
            s += _data[i] + " ";
        s += "count = " + _data.Count;
        return s;
    }

    public bool IsConsistent()
    {
        // is the heap property true for all data?
        if (_data.Count == 0) return true;
        int li = _data.Count - 1; // last index
        for (int pi = 0; pi < _data.Count; ++pi) // each parent index
        {
            int lci = 2 * pi + 1; // left child index
            int rci = 2 * pi + 2; // right child index

            if (lci <= li && _data[pi].CompareTo(_data[lci]) > 0) return false; // if lc exists and it's greater than parent then bad.
            if (rci <= li && _data[pi].CompareTo(_data[rci]) > 0) return false; // check the right child too.
        }
        return true; // passed all checks
    }
}