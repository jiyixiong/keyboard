using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;

class WordNode{ //单词节点，包含单词字符串及其词频
	public string word;
	public int count;

	public WordNode(string w, int c) {
		this.word = w;
		this.count = c;
	}
}

class Node { //树的结点，有当前节点的子节点、当前节点的单词节点的列表
	public List<WordNode> words;
	public Dictionary<string, Node> child;

	public Node() {
		words = new List<WordNode>();
		child = new Dictionary<string, Node>();
	}
}

public class WordNodeComparer : IComparer<Person>
{
    public int Compare(WordNode x, WordNode y)
    {
        if (x == null && y == null) return 0;
        if (x == null) return -1;
        if (y == null) return 1;

        if (x.count > y.count) return 1;
        else if(x.count < y.count) return -1; 

        return 0;
    }
}

class NumToStrList {
	private Node wordTree; 			//单词树的根节点
	private Hashtable numToChar;
	private Hashtable charToNum;	//字母转化为九宫格对应的数字
	private string cacheNumStr; 	//当前的数字串
	private List<Node> cachePtr; 	//当前保存的指针
	private List<string> cacheWords;//当前的词的缓存

	private List<string> nullStrList;//空串

    private void initTransform() {
    	numToChar =new Hashtable();
    	numToChar.Add("2", "a");
    	numToChar.Add("3", "d");
    	numToChar.Add("4", "i");
    	numToChar.Add("5", "j");
    	numToChar.Add("6", "m");
    	numToChar.Add("7", "p");
    	numToChar.Add("8", "t");
    	numToChar.Add("9", "w");
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
		nullStrList = new List<string>();
		wordTree = new Node();

		//读取字频文件，建树
		StreamReader sr = new StreamReader("lexicon.txt", Encoding.Default);
		string line;
		while((line = sr.ReadLine()) != null) {
			Node curNode = wordTree;
			string[] tmp = line.Split(' ');
			foreach(char cha in tmp[0]) {
				string num = charToNum[cha.ToString()].ToString();
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

		//往下搜一层
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
		curDeepList.Sort(new WordNodeComparer());

		cachePtr = newPtrList;
		foreach(WordNode wordNode in curDeepList) { 
			cacheWords.Add(wordNode.word);
		}
	}

	private List<string> getFromCache(string numStr, int level = 0) {
		List<string> results = new List<string>();
		if (numStr == cacheNumStr) {
			if(cacheWords.Count > 5*level+4) { //如果足够
				for(int i= 0; i<5; i++)
					results.Add(cacheWords[5*level+i]);
				return results;
			} else if(cachePtr.Count > 0) { //如果不够但还有子节点
				updateCacheBySearchDeeper();
				return getFromCache(numStr, level);
			} else { //彻底没有了
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

			//清除缓存
			cachePtr.Clear();
			cacheNumStr = numStr;
			cacheWords.Clear();

			//找到对应节点
			Node curNode = wordTree;
			foreach (char num in numStr) {
				if (curNode.child.ContainsKey(num.ToString())) {
					curNode = curNode.child[num.ToString()];
				} else {
					List<string> errorList = new List<string>();
					string returnStr = "";
					for(char cnum in numStr)
						returnStr = returnStr + numToChar[cnum.ToString()];
					errorList.Add(returnStr);
					return errorList;
				}
			}

			//加载到缓存，并读取缓存并返回
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