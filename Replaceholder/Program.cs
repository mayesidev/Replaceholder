using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace Replaceholder
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var xmlFilePath = args[0];
			var templateFilePath = args[1];
			var outputFilePath = args[2];

			if( _ValidateAndSetProgramInputs(xmlFilePath, templateFilePath, outputFilePath) )
			{
				var config = _ParseConfigXml();
				var templateContent = File.ReadAllLines(mTemplateFilePath);

				var result = _ReplaceContent(config, templateContent);
				if (string.IsNullOrEmpty(mOutputFilePath))
				{
					Console.Out.Write(result);
				}
				else
				{
					File.WriteAllText(mOutputFilePath, result);
				}
			}
		}

		private static string _ReplaceContent(Config config, string[] template)
		{
			string result = string.Empty;
			foreach (var line in template)
			{
				var replacedLine = line;
				foreach(var content in config.Content )
				{
					switch (content.DataType)
					{
						case DataTypeValue.Text:
							replacedLine = _ReplaceText(content, replacedLine);
							break;
						case DataTypeValue.Path:

							break;
						case DataTypeValue.Placeholder:
							break;
						default:
							var errorMessage = string.Format("DataTypeValue of '{0}' not supported.", content.DataType);
							throw new InvalidOperationException(errorMessage);
					}
				}
				result = string.Concat(result, replacedLine, Environment.NewLine);
			}

			return result;
		}

		private static string _ReplaceText(Content content, string line)
		{
			if(line.Contains(content.KeyName))
			{
				line = line.Replace(content.KeyName, content.Value);
			}

			return line;
		}

		private static string _ReplacePath(Content content, string line)
		{
			throw new NotImplementedException();
		}

		private static string _ReplacePlaceholder(Content content, string line)
		{
			throw new NotImplementedException();
		}

		private static Config _ParseConfigXml()
		{
			var serializer = new XmlSerializer(typeof(Config));
			var fileStream = File.OpenRead(mXmlFilePath);
			var reader = XmlReader.Create(fileStream);
			var xml = serializer.Deserialize(reader);

			return (Config)xml;
		}

		private static string mXmlFilePath;
		private static string mTemplateFilePath;
		private static string mOutputFilePath;

		private static bool _ValidateAndSetProgramInputs(string xmlFilePath, string templateFilePath, string outputFilePath)
		{
			var xmlPathIsNull = string.IsNullOrWhiteSpace(xmlFilePath);
			var templatePathIsNull = string.IsNullOrWhiteSpace(templateFilePath);
			var outputPathIsNull = string.IsNullOrWhiteSpace(outputFilePath);

			var validInputs = false;
			if(!xmlPathIsNull && !templatePathIsNull)
			{
				if (File.Exists(xmlFilePath))
				{
					mXmlFilePath = xmlFilePath;

					if (File.Exists(templateFilePath))
					{
						mTemplateFilePath = templateFilePath;

						mOutputFilePath = !outputPathIsNull && File.Exists(outputFilePath)
							? outputFilePath
							: string.Empty;

						validInputs = true;
					}
				}
			}
			return validInputs;
		}

	}
}
