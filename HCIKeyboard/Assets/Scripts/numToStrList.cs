using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;


class WordNode{
	public string word;
	public int count;

	public WordNode(string w, int c)
	{
		this.word = w;
		this.count = c;
	}
}

class Node
{
	public List<WordNode> words;
	public Dictionary<string, Node> child;

	public Node()
	{
		words = new List<WordNode>();
		child = new Dictionary<string, Node>();
	}
}

class NumToStrList {
	private Node wordTree;
	private Hashtable charToNum;
	private string cacheNumStr;
	private List<Node> cachePtr;
	private List<string> cacheWords;

	private List<string> nullStrList = new List<string> {"", "", "", "", ""};

    private void initTransform() {
        charToNum = new Hashtable();
        charToNum.Add("a", "2");
        charToNum.Add("b", "2");
        charToNum.Add("c", "2");
        charToNum.Add("d", "3");
        charToNum.Add("e", "3");
        charToNum.Add("f", "3");
        charToNum.Add("g", "4");
        charToNum.Add("h", "4");
        charToNum.Add("i", "4");
        charToNum.Add("j", "5");
        charToNum.Add("k", "5");
        charToNum.Add("l", "5");
        charToNum.Add("m", "6");
        charToNum.Add("n", "6");
        charToNum.Add("o", "6");
        charToNum.Add("p", "7");
        charToNum.Add("q", "7");
        charToNum.Add("r", "7");
        charToNum.Add("s", "7");
        charToNum.Add("t", "8");
        charToNum.Add("u", "8");
        charToNum.Add("v", "8");
        charToNum.Add("w", "9");
        charToNum.Add("x", "9");
        charToNum.Add("y", "9");
        charToNum.Add("z", "9");
    }

    public NumToStrList () {
		initTransform();
		cachePtr = new List<Node>();
		cacheWords = new List<string>();
		wordTree = new Node();

		StreamReader sr = new StreamReader("lexicon.txt", Encoding.Default);
		string line;
		while((line = sr.ReadLine()) != null) {
			Node curNode = wordTree;
			string[] tmp = line.Split(' ');
			foreach(char cha in tmp[0]) {
				string num = charToNum[cha].ToString();
				if(!curNode.child.ContainsKey(num)) {
					Node newNode = new Node();
					curNode.child.Add(num, newNode);
				}
				curNode = curNode.child[num];
			}
			if(curNode.words == null) {
				curNode.words = new List<WordNode>();
			}
			curNode.words.Add(new WordNode(tmp[0], int.Parse(tmp[1])));
		}
	}

	private void updateCacheBySearchDeeper() {
		ArrayList curDeepList = new ArrayList();
		List<Node> newPtrList = new List<Node>();

		foreach (Node parentNode in cachePtr) {
			foreach(string key in parentNode.child.Keys) {
				Node node = parentNode.child[key];
				newPtrList.Add(node);
				if(node.words != null) {
					foreach (WordNode wordNode in node.words) {
						curDeepList.Add(wordNode);
					}
				}
			}
		}

		cachePtr = newPtrList;
		foreach(WordNode wordNode in curDeepList) { 
			cacheWords.Add(wordNode.word);
		}
	}

	private List<string> getFromCache(string numStr, int level = 0) {
		List<string> results = new List<string>();
		if (numStr == cacheNumStr) {
			if(cacheWords.Count > 5*level+4) {
				for(int i= 0; i<5; i++)
					results.Add(cacheWords[5*level+i]);
				return results;
			} else if(cachePtr.Count > 0) {
				updateCacheBySearchDeeper();
				return getFromCache(numStr, level);
			} else {
				for(int i= 5*level; i<cacheWords.Count; i++)
					results.Add(cacheWords[i]);
				return results;
			}
		} else {
			return getStrList(numStr, level);
		}
	}

	public List<string> getStrList(string numStr, int level = 0) {
		if (level > 0) {
			return getFromCache(numStr, level);
		} else if (level == 0) {
			cachePtr.Clear();
			cacheNumStr = numStr;
			cacheWords.Clear();

			Node curNode = wordTree;
			foreach (char num in numStr) {
				if (curNode.child.ContainsKey(num.ToString())) {
					curNode = curNode.child[num.ToString()];
				} else {
					return nullStrList;
				}
			}

			if (curNode.words != null) {
				List<WordNode> wordsList = curNode.words;
				foreach(WordNode wordNode in wordsList){ 
					cacheWords.Add(wordNode.word);
				}
				cachePtr.Add(curNode);
				return getFromCache(numStr);
			} else {
				cachePtr.Add(curNode);
				return getFromCache(numStr);
			}
		}
		return nullStrList;
	}
	
}