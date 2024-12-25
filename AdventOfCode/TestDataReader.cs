using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;

namespace AdventOfCode
{
	public class TestDataReader
	{
		private static readonly HttpClient client;

		private static HttpClient Client => client ?? CreateClient();

		public static string GetTestData(string day, int? testIndex)
		{
			var fileName = InputDataFileName(testIndex, day);
			string testData;
			var fileExists = File.Exists(fileName);

            if (!fileExists && !testIndex.HasValue)
			{
				testData = Client.GetStringAsync($"https://adventofcode.com/{Program.CurrentYearNumber}/day/{day}/input").Result;
				File.WriteAllText(fileName, testData);
			}
			else if (fileExists)
			{
				testData = File.ReadAllText(fileName);
			}
			else
			{
                throw new Exception($"No test data stored for test number {testIndex.Value}");
            }

			if (string.IsNullOrEmpty(testData))
			{
				throw new Exception($"Failed to retrieve test data for problem {day}");
			}

			return testData;
		}

		public static void RetrieveTestData(string day)
		{
			var html = Client.GetStringAsync($"https://adventofcode.com/{Program.CurrentYearNumber}/day/{day}").Result;
			const int MinLength = 10;
			const string StartTag = "<code>";
            const string EndTag = "</code>";
			var blacklist = new string[]
			{
				"<em>",
				"</em>"
			};

            var index = html.IndexOf(StartTag);
			var keptCount = 0;
			while (index != -1)
			{
				var end = html.IndexOf(EndTag);
				var testData = html.Substring(index + StartTag.Length, end - index - StartTag.Length);
				html = html.Substring(end + EndTag.Length);
				index = html.IndexOf(StartTag);
                blacklist.ForEach(x => testData = testData.Replace(x, string.Empty));
                if (testData.Length < MinLength)
				{
					continue;
				}

				Console.Clear();
				Console.WriteLine(testData);
				Console.WriteLine("Keep? Y/N/Abort");
				var answer = Console.ReadLine();
				if (answer.Equals("y", StringComparison.OrdinalIgnoreCase))
				{
					var fileName = InputDataFileName(keptCount + 1, day);
					keptCount++;
                    File.WriteAllText(fileName, testData);
                }
				else if (answer.Equals("abort", StringComparison.OrdinalIgnoreCase))
				{
					return;
				}
			}
        }

		private static HttpClient CreateClient()
		{
			const string APIKeyFilePath = "APIKey.txt";
            if (!File.Exists(APIKeyFilePath))
			{
				throw new Exception("A session key stored in APIKey.txt is required.");
			}

			var sessionKey = File.ReadAllText(APIKeyFilePath);
			Uri uri = new Uri("https://adventofcode.com");
			var handler = new HttpClientHandler
			{
				CookieContainer = new CookieContainer()
			};

			HttpClient client = new HttpClient(handler);
            handler.CookieContainer.Add(uri, new Cookie("session", sessionKey));
			return client;
		}

		private static string InputDataFileName(int? testIndex, string day) => $"{Program.CurrentYearNumber}/" + testIndex switch
		{
			null => $"Input{day}.txt",
			0 => $"Test{day}.txt",
            1 => $"Test{day}.txt",
            _ => $"Test{day}-{testIndex.Value}.txt."
		};
    }
}
