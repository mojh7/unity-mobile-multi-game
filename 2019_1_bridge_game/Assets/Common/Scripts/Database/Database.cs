using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Database<T> : ScriptableObject where T : ScriptableObject
{
    public List<T> dataList = new List<T>();
    private Dictionary<string, T> dataDictionary = new Dictionary<string, T>();
    private static Database<T> instance = null;

    public static Database<T> Instance
    {
        get
        {
            if (!instance)
                instance = FindObjectOfType<Database<T>>();

            return instance;
        }
        set { instance = value; }

    }

    public void Instantiate()
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            T temp = dataList[i];
            dataDictionary.Add(temp.name, temp);
        }
    }

    public T GetData(string _name)
    {
        return dataDictionary[_name];
    }

    public T GetData(int i)
    {
        return dataList[i];
    }
}