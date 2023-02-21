using System;
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class FlowList<T>
{

    public class Item
    {
        public int listIndex { get; set; }
        public int iterationIndex { get; set; }
        public RectTransform rectTransform => transform as RectTransform;
        public GameObject gameObject;
        public T component;
        public Transform transform => gameObject.transform;
        public List<Component> otherComponents = new();
    }
    public FlowList()
    {

    }

    Transform container => template.transform.parent;
    public GameObject template;
    [System.Obsolete]
    public Comparison<T> comparison;

    public int count => items.Count;

    List<Item> items = new();

    [System.Obsolete]
    public void sort() => sort(comparison);
    public void sort(Comparison<T> comparison)
    {
        items.Sort((x, y) => comparison(x.component, y.component));
        for (int i = 0; i < items.Count; i++)
        {
            items[i].listIndex = i;
            items[i].transform.SetSiblingIndex(i);
        }
    }
    public void iterate(int count, System.Action<Item> action)
    {
        for (int i = 0; i < count; i++)
        {
            var x = get(i);
            x.iterationIndex = i;
            action(x);
        }
    }
    public Item get(int index)
    {
        if (template.activeSelf)
            template.gameObject.SetActive(false);
        if (index >= items.Count)
        {
            for (int i = items.Count; i <= index; i++)
            {
                var g = GameObject.Instantiate(template);
                g.SetActive(true);
                g.transform.SetParent(container, false);
                items.Add(new Item
                {
                    listIndex = index,
                    gameObject = g,
                    component = g.GetComponent<T>(),
                });
            }
        }
        return items[index];
    }

    public void insure()
    {
        if (!template)
            return;
        if (template.GetComponent<T>() == null)
            Debug.LogError(template + " Template not has " + typeof(T).ToString(), template);
    }



}