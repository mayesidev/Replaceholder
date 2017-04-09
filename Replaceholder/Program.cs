using System;
using System.IO;

namespace Replaceholder
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var xmlFilePath = string.Empty;
			var templateFilePath = string.Empty;
			var outputFilePath = string.Empty;

			var numberOfSuppliedArgs = args.Length;
			if (numberOfSuppliedArgs >= 1)
			{
				xmlFilePath = args[0];
			}
			if (numberOfSuppliedArgs >= 2)
			{
				templateFilePath = args[1];
			}
			if (numberOfSuppliedArgs >= 3)
			{
				outputFilePath = args[2];
			}

			xmlFilePath = _VerifyInput(xmlFilePath, "XML");
			templateFilePath = _VerifyInput(templateFilePath, "template");

			var replaceResult = ReplaceholderInstance.DoReplaceContent(xmlFilePath, templateFilePath);

			if (_IsInputValid(outputFilePath))
			{
				var errorMessage = _TryWriteToFile(outputFilePath, replaceResult);
				if (!errorMessage.Equals(string.Empty))
				{
					Console.Out.WriteLine(errorMessage);
				}
			}
			else
			{
				Console.Out.WriteLine(replaceResult);
			}
		}

		private static bool _IsInputValid(string input)
		{
			return !string.IsNullOrWhiteSpace(input);
		}

		private static string _PromptForInput(string inputName)
		{
			Console.Out.WriteLine("Please supply a valid value for the {0} file path:", inputName);
			return Console.ReadLine();
		}

		private static string _TryWriteToFile(string path, string contents)
		{
			var errorMessage = string.Empty;
			try
			{
				File.WriteAllText(path, contents);
			}
			catch (Exception ex)
			{
				errorMessage = string.Format("Exception occurred when trying to write to file.{0}Exception was: '{1}'{0}File path was: '{2}'", Environment.NewLine, ex.Message, path);
			}
			return errorMessage;
		}

		private static string _VerifyInput(string input, string inputName)
		{
			while (!_IsInputValid(input))
			{
				input = _PromptForInput(inputName);
			}
			return input;
		}
	}
}
