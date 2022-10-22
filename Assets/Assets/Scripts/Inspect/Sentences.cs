using System.Collections.Generic;

[System.Serializable]
public class Sentences
{
    public List<string> myList;

    public Sentences(string[] text)
    {
        myList = new List<string>(text);
    }

    public string this[int index]
    {
        get => myList[index];
        set => myList[index] = value;
    }

    public void Add(string text)
    {
        myList.Add(text);
    }

    public void RemoveAt(int idx)
    {
        myList.RemoveAt(idx);
    }

    public string[] ToArray()
    {
        return myList.ToArray();
    }
}
