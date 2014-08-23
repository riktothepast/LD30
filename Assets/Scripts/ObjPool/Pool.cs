using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;


//Pool Class from http://jason-mitchell.com/wp7/generic-pool-class-for-reusing-objects/

public class Pool<T>
{
    private readonly List<T> items = new List<T>();
    private readonly Queue<T> freeItems = new Queue<T>();

    private readonly Func<T> createItemAction;

    public Pool(Func<T> createItemAction)
    {
        this.createItemAction = createItemAction;
    }



    public void Preallocate(int itemCount)
    {
        T[] items = new T[itemCount];
        for (int i = 0; i < itemCount; ++i)
        {
            items[i] = GetFreeItem();
        }

        for (int i = 0; i < itemCount; ++i)
        {
            FlagFreeItem(items[i]);
        }
    }

    public void FlagFreeItem(T item)
    {
            freeItems.Enqueue(item);
    }

    public T GetFreeItem()
    {
        if (freeItems.Count == 0)
        {
            T item = createItemAction();
            items.Add(item);
            return item;
        }

        return freeItems.Dequeue();
    }

    public bool hasFreeItems() {
        return freeItems.Count > 0 ? true : false;
    }

    public int GetFreeItemCount() {
        return freeItems.Count;
    }

    public List<T> Items
    {
        get { return items; }
    }

    public Queue<T> FreeItems
    {
        get { return freeItems; }
    }

    public void Clear()
    {
        items.Clear();
        freeItems.Clear();
    }
}