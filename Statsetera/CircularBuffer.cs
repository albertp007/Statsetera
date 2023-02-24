using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Statsetera;

public class CircularBuffer<T> : ICollection<T>
where T: IComparable<T>
{
    private int head = -1;
    private int end = -1;
    private int count = 0;
    private T[] queue;
    public CircularBuffer(int size)
    {
        queue = new T[size];
        Size = size;    // only possible in constructor
    }
    public int Size { get; }
    public int Count { get => count; }
    public bool IsReadOnly { get; } = false;
    public void Add(T c)
    {
        end = advance(end);
        queue[end] = c;
        count++;
        if ( count > Size )
        {
            count = Size;
        }
        if ( head == -1 || head == end ) 
        {
            head = advance(head);
        }
    }
    public void Clear()
    {
        Reset();
    }
    public bool Contains(T item)
    {
        foreach( T element in this )
        {
            if ( item.CompareTo(element) == 0 )
            {
                return true;
            }
        }
        return false;
    }
    public void CopyTo(T[] array, int n)
    {
        int i = n;
        // since the class itself implements ICollection and IEnumerable, we can
        // use foreach on *this*
        foreach ( T element in this )
        {
            array[i++] = element;
        }
    }
    public IEnumerator<T> GetEnumerator()
    {
        if ( head >= 0 && end >= 0 )
        {
            int i = 0;
            for (i = head; i != end; i = advance(i))
            {
                yield return queue[i];
            }
            // there is one last element when head of queue is equal to end of queue
            yield return queue[i];
        }
    }
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
    bool ICollection<T>.Remove(T item)
    {
        if( item.CompareTo(queue[head]) == 0 )
        {
            Pop();
            return true;
        }
        return false;
    }
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.Append($"Count: {count} ");
        foreach( T element in this )
        {
            sb.Append($"{element},");
        }
        if (sb.Length > 0)
        {
            sb.Remove(sb.Length-1, 1);
        }
        return sb.ToString();
    }
    public T Pop()
    {
        if (head == -1)
        {
           throw new InvalidOperationException("Container is empty");
        }
        else
        {
            T result = queue[head];
            if (head == end)
            {
                // last element in the queue, clear the queue
                Reset();
            }
            else {
                head = advance(head);
            }
            count--;
            return result;
        }
    }
    private int advance(int i) => (i+1) % Size;
    private void Reset()
    {
        head = -1;
        end = -1;
    }

}