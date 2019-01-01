using System.IO;
using System.Text;
using System.Collections;

struct Node {
	HashTable words;
	Node* child[10];
}

class NumToStrList {

	private HashTable wordTree;
	// private Node wordTree;
	private HashTable charToNum;
	private HashTable cache;

	private string[] nullStrList = {"", "", "", "", ""};

	public NumToStrList () {
		initTransform();
		cache.Add("ptr", new ArrayList());
		cache.Add("numStr", numStr);
		cache.Add("results", new ArrayList());
		cache = new HashTable();
		StreamReader sr = new StreamReader("lexicon.txt", Encoding.Default);
		string line;
		while((line = sr.ReadLine()) != null) {
			HashTable curNode = wordTree;
			string[] tmp = line.toString().Split(' ');
			foreach(char cha in tmp[0]) {
				string num = charToNum[cha];
				if(!curNode.ContainsKey(num)) {
					HashTable newNode = new HashTable();
					curNode.Add(num, newNode);
				}
				curNode = curNode[num];
			}
			if(!curNode.ContainsKey("words")) {
				HashTable newNode = new HashTable();
				curNode.Add("words", newNode);
			}
			curNode["words"].Add(tmp[0], int.Parse(tmp[1]));
		}
	}

	private void updateCacheBySearchDeeper() {
		HashTable curDeepTable = new HashTable();
		ArrayList curDeepList = new ArrayList();
		cache["ptr"].clear();
		ArrayList newPtrList = cache["ptr"];

		foreach (HashTable parentNode in cache["ptr"]) {
			foreach(string key in parentNode) {
				if(key != "words") {
					HashTable node = parentNode[key]
					newPtrList.Add(node);
					if(node.ContainsKey("words")) {
						foreach (DictionaryEntry de in node["words"]) {
							curDeepTable.Add(de.Key.toString(), de.Value);
						}
					}
				}
			}
		}
		List<Map.Entry<String,String>> list = new ArrayList<Map.Entry<String,String>>(curDeepTable.entrySet());
		Collections.sort(list, new Comparator<Map.Entry<String,String>>() {
    		//升序排序
    		public int compare(Entry<String, String> o1, Entry<String, String> o2) {
        		return o1.getValue().compareTo(o2.getValue());
    		}    
		});
		for(Map.Entry<String,String> mapping:list){ 
			cache["cache"].Add(mapping.getValue());
		}
	}

	private string[] getFromCache(string numStr, int level = 0) {
		if (numStr == cache["numStr"]) {
			if(cache["cache"].Count > 5*level+4) {
				return ;
			} else if(cache["ptr"].Count > 0) {
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
			// cache初始化
			// cache.Add("ptr", new Vector<HashTable>());
			// cache.Add("numStr", numStr);
			// cache.Add("results", new ArrayList());
			cache["ptr"].clear();
			cache["numStr"] = numStr;
			cache["cache"].clear();

			HashTable curNode = wordTree;
			foreach(char num in numStr) {
				if(curNode.ContainsKey(num)) {
					curNode = curNode[num];
				} else {
					return nullStrList;
				}
			}

			if(curNode.ContainsKey("words")) {
				HashTable wordsTable = curNode["words"];
				List<Map.Entry<String,String>> list = new ArrayList<Map.Entry<String,String>>(wordsTable.entrySet());
        		Collections.sort(list,new Comparator<Map.Entry<String,String>>() {
            		//升序排序
            		public int compare(Entry<String, String> o1, Entry<String, String> o2) {
                		return o1.getValue().compareTo(o2.getValue());
            		}    
        		});
        		for(Map.Entry<String,String> mapping:list){ 
        			cache["cache"].Add(mapping.getValue());
        		}
        		cache["ptr"].Add(curNode);
        		return getFromCache(numStr);
			} else {
				cache["ptr"].Add(curNode);
        		return getFromCache(numStr);
			}
		}
	}

	private void initTransform() {
		charToNum = new HashTable();
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
}