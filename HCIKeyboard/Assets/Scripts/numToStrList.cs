using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;


class WordNode{
	string word;
	int count;

	public WordNode(string w, int c)
	{
		this.word = w;
		this.count = c;
	}
}

class Node
{
	public List<WordNode> words;
	public Dictionary<int, Node> child;

	public Node()
	{
		words = new List<WordNode>();
		child = new Dictionary<int, Node>();
	}
}

class NumToStrList {
	private Node wordTree;
	private Hashtable charToNum;
	private string cacheNumStr;
	private List<Node> cachePtr;
	private List<WordNode> cacheWords;

	private string[] nullStrList = {"", "", "", "", ""};

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
		cacheWords = new List<WordNode>();
		wordTree = new Node();

		StreamReader sr = new StreamReader("lexicon.txt", Encoding.Default);
		string line;
		while((line = sr.ReadLine()) != null) {
			Node curNode = wordTree;
			string[] tmp = line.toString().Split(' ');
			foreach(char cha in tmp[0]) {
				string num = charToNum[cha];
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
			cacheWords.Add(wordNode.words);
		}
	}

	private string[] getFromCache(string numStr, int level = 0) {
		if (numStr == cacheNumStr) {
			if(cacheWords.Count > 5*level+4) {
				return ;
			} else if(cachePtr.Count > 0) {
				updateCacheBySearchDeeper();
				return getFromCache(numStr, level);
			} else {
				return ;
			}
		} else {
			return getStrList(numStr, level);
		}
	}

	public string[] getStrList(string numStr, int level = 0) {
		if (level > 0) {
			return getFromCache(numStr, level);
		} else if (level == 0) {
			cachePtr.clear();
			cacheNumStr = numStr;
			cacheWords.clear();

			Node curNode = wordTree;
			foreach (char num in numStr) {
				if (curNode.child.ContainsKey(num)) {
					curNode = curNode.child[num];
				} else {
					return nullStrList;
				}
			}

			if (curNode.words != null) {
				List<WordNode> wordsList = curNode.words;
				for(WordNode wordNode : wordList){ 
					cacheWords.Add(wordNode.word);
				}
				cachePtr.Add(curNode);
				return getFromCache(numStr);
			} else {
				cachePtr.Add(curNode);
				return getFromCache(numStr);
			}
		}
	}
	
}