using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace Replaceholder
{
	public static class ReplaceholderInstance
	{
		public static string DoReplaceContent(string xmlFilePath, string templateFilePath)
		{
			var doesXmlFileExist = _FileExistsAtPath(xmlFilePath);
			var doesTemplateFileExist = _FileExistsAtPath(templateFilePath);

			var result = string.Empty;
			if (doesXmlFileExist && doesTemplateFileExist)
			{
				try
				{
					var config = _ParseConfigXml(xmlFilePath);
					var templateContent = File.ReadAllLines(templateFilePath);

					result = _ReplaceContent(config, templateContent);
				}
				catch (Exception ex)
				{
					result = string.Format("Exception occurred when trying to replace content.{0}Exception was: '{1}'{0}XML file path was: '{2}'{0}Template file path was: '{3}'", Environment.NewLine, ex.Message, xmlFilePath, templateFilePath);
				}
			}
			else
			{
				result = "Input file paths were invalid.";
				if (!doesXmlFileExist)
				{
					var errorMessage = string.Format("{0}XML file does not exist at path: '{1}'", Environment.NewLine, xmlFilePath);
					result = string.Concat(result, errorMessage);
				}

				if (!doesTemplateFileExist)
				{
					var errorMessage = string.Format("{0}Template file does not exist at path: '{1}'", Environment.NewLine, templateFilePath);
					result = string.Concat(result, errorMessage);
				}
			}
			return result;
		}

		private static bool _FileExistsAtPath(string filePath)
		{
			return !string.IsNullOrWhiteSpace(filePath) && File.Exists(filePath);
		}

		private static Config _ParseConfigXml(string xmlFilePath)
		{
			var serializer = new XmlSerializer(typeof(Config));
			var fileStream = File.OpenRead(xmlFilePath);
			var reader = XmlReader.Create(fileStream);
			var xml = serializer.Deserialize(reader);

			return (Config)xml;
		}

		private static string _ReplaceContent(Config config, string[] template)
		{
			string result = string.Empty;
			foreach (var line in template)
			{
				var replacedLine = line;
				foreach (var content in config.Content)
				{
					switch (content.DataType)
					{
						case DataTypeValue.Text:
							replacedLine = _ReplaceText(content, replacedLine);
							break;
						case DataTypeValue.Path:
							_ReplacePath(content, replacedLine);
							break;
						case DataTypeValue.Placeholder:
							_ReplacePlaceholder(content, replacedLine);
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

		private static string _ReplacePath(Content content, string line)
		{
			throw new NotImplementedException();
		}

		private static string _ReplacePlaceholder(Content content, string line)
		{
			throw new NotImplementedException();
		}

		private static string _ReplaceText(Content content, string line)
		{
			if (line.Contains(content.KeyName))
			{
				line = line.Replace(content.KeyName, content.Value);
			}
			return line;
		}
	}
}
