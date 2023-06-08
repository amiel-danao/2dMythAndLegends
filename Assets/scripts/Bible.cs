using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Linq;

public class Bible
{
    public List<string> All_words { get; set; }
    public List<string> Bad_words { get; set; }
    public Dictionary<string, bool> All_words_final { get; set; }
    public Dictionary<string, bool> Bad_words_final { get; set; }

    public List<string> bible_list { get; set; }
    public Dictionary<string, bool> Bible_words { get; set; }

    public Bible()
    {
        All_words = new List<string>();
        Bad_words = new List<string>();
        bible_list = new List<string>();
        ReadAllWordsFromFile();
    }

    public void ReadWordsFromFile()
    {
        AddWords(File.ReadAllLines("assets/resources/word_list.txt"));
        AddWords(File.ReadAllLines("assets/resources/generic_word_list.txt"));

        WriteAllWordsToFile();
    }

    private void AddWords(string[] newWords)
    {
        All_words.AddRange(newWords);
    }

    private void ReadAllWordsFromFile()
    {
        //Bad_words.AddRange(File.ReadAllLines("assets/resources/bad_words.txt"));
        //Bad_words_final = Bad_words.ToArray().Select(s => s.ToUpper().Trim()).Distinct().ToDictionary(s => s, s => true);

        //bible_list.AddRange(File.ReadAllLines("assets/resources/bible_words.txt"));
        //Bible_words = bible_list.ToArray().Select(s => s.ToUpper().Trim()).Distinct().ToDictionary(s => s, s => true);

        All_words.AddRange(File.ReadAllLines("assets/resources/all_words.txt"));
        //All_words.AddRange(File.ReadAllLines("assets/resources/words_alpha.txt"));
        Dictionary<string, bool> dicTwo = All_words.ToArray().Select(s => s.ToUpper().Trim()).Distinct().ToDictionary(s => s, s => true);

        //var a = dicTwo.Where(x => !Bad_words_final.ContainsKey(x.Key));

        All_words_final = dicTwo.ToDictionary(x => x.Key, x => x.Value);
    }

    private void WriteAllWordsToFile()
    {
        All_words.Sort();
        All_words_final = All_words.Distinct().ToDictionary(s=>s, s=>true);
        Debug.Log("WriteAllWordsToFile");
        string path = "assets/resources/all_words.txt";
        File.WriteAllLines(path, All_words);        
    }

    public bool ValidWord(string word)
    {
        return (!Bad_words_final.ContainsKey(word) && All_words_final.ContainsKey(word));
    }
}
