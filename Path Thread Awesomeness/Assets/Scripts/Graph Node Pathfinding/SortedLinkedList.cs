using System;
using System.Collections.Generic;

/// <summary>
/// A sorted linked list
/// Used for a list of search nodes in this case
/// </summary>
public class SortedLinkedList<T> : LinkedList<T> where T : IComparable
{
    private LinkedListNode<T> head;
    private int count;

    #region Constructor

    public SortedLinkedList() : base()
    {
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds the given item to the list
    /// </summary>
    /// <param name="item">item to add to list</param>
    public void Add(T item)
    {
        // adding to empty list
        if (Count == 0)
        {
            AddFirst(item);
        }
        else if (First.Value.CompareTo(item) >= 0) //if first search node distance value is equal to or greater than item node distance value
        {
            // adding before head
            AddFirst(item);
        }
        else
        {
            // find place to add new node
            LinkedListNode<T> previousNode = null;
            LinkedListNode<T> currentNode = First;

            while (currentNode != null &&
                   currentNode.Value.CompareTo(item) < 0) //if current node distance value is less than item node distance value
            {
                previousNode = currentNode;
                currentNode = currentNode.Next;
            }

            // link in new node between previous node and current node
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
