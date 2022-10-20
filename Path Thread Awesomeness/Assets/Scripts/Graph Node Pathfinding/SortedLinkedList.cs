using System;
using System.Collections.Generic;

/// <summary>
/// A sorted linked list
/// Used for a list of search nodes in this case
/// </summary>
public class SortedLinkedList<T> : LinkedList<T> where T : IComparable
{
    #region Fields

    private LinkedListNode<T> head;
    private int count;

    #endregion

    #region Constructor

    public SortedLinkedList() : base()
    {
    }

    #endregion

    #region Public Methods

    public void Add(T item)
    {
        // adding to empty list
        if (Count == 0)
        {
            AddFirst(item);
        }
        else if (First.Value.CompareTo(item) >= 0)
        {
            // adding before head
            AddFirst(item);
        }
        else
        {
            LinkedListNode<T> previousNode = null;
            LinkedListNode<T> currentNode = First;

            while (currentNode != null &&
                   currentNode.Value.CompareTo(item) < 0)
            {
                previousNode = currentNode;
                currentNode = currentNode.Next;
            }

            AddAfter(previousNode, item);
        }
    }

    /// <summary>
    /// Repositions the given item in the list by moving it
    /// forward in the list until it's in the correct
    /// position. This is necessary to keep the list sorted
    /// when the value of the item changes
    /// </summary>
    public void Reposition(T item)
    {
        // move item forward into correct position
        LinkedListNode<T> currentNode = Find(item);
        while (currentNode.Previous != null &&
               currentNode.Value.CompareTo(currentNode.Previous.Value) < 0)
        {
            currentNode.Value = currentNode.Previous.Value;
            currentNode.Previous.Value = item;
            currentNode = currentNode.Previous;
        }
    }

    #endregion
}