using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace promit_test
{
	public class Document : IEnumerable<Word>
	{
		private readonly Dictionary<string, int> wordFrequency;
		private readonly List<Word> wordList;

		public Document(string text)
		{
			wordFrequency = CreateFrequency(text.ToLower());
			wordList = wordFrequency.Where(frequency => frequency.Value >= 3)
				.Select(el => new Word(el.Key, el.Value))
				.ToList();
		}

		private Dictionary<string, int> CreateFrequency(string text)
		{
			var result = new Dictionary<string, int>();
			for (var i = 0; i < text.Length; i++)
			{
				var (word, lastIndex) = GetNextWord(text, i);
				i = lastIndex;
				if (word.Length < 3 || word.Length > 15)
					continue;
				
				if(!result.ContainsKey(word))
					result.Add(word, 0);

				result[word]++;
			}

			return result;
		}
		
		public static Document ReadDocument()
		{
			Console.WriteLine("Введите полный путь к файлу");
			var path =  Console.ReadLine();
			try
			{
				var sr = new StreamReader(path, Encoding.UTF8);
				var text = sr.ReadToEnd();
				return new Document(text);
			}
			catch (FileNotFoundException e)
			{
				throw new FileNotFoundException("Файл не найден");
			}
		}
		
		private (string word, int lastIndex) GetNextWord(string text, int startIndex)
		{
			var i = startIndex;
			while (i < text.Length && !char.IsLetter(text[i])) 
				i++;

			if (i == text.Length)
				return ("", i);

			var wordBuilder = new StringBuilder();

			while (i < text.Length && char.IsLetter(text[i]))
			{
				wordBuilder.Append(text[i]);
				i++;
			}

			return (wordBuilder.ToString(), i);
		}
		
		public IEnumerator<Word> GetEnumerator()
		{
			foreach (var word in wordList)
				yield return word;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}