using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;

class Word: IComparer <Word>
{
    string word;
    int num;
    
    public Word(string w, int n)
    {
        this.word = w;
        this.num = n;
    }
     int IComparer<Word>.Compare(Word x, Word y) 
    {
        return x.num - y.num;
    }

}
public class TestArray  {

    private List<Word> words;
	public TestArray()
    {
        this.words.Add(new Word("a", 11));
    }
}
