using System;
using UnityEngine;
using System.Collections.Generic;
using System.Text;
using LitJson;

using System.Reflection;

public class ExcelLoader
{
	public static void Init()
	{
		TextAsset asset = Resources.Load<TextAsset>("Excel/excel_index");
		string[] lines = GetLines(asset);

		for (int i = 1; i < lines.Length; ++i)
		{
			string line = lines[i];
			string[] datas = line.Split('\t');
			string className = datas[1];
			asset = Resources.Load<TextAsset>("Excel/config/" + className);
			JsonData data = JsonMapper.ToObject(asset.text);

			JsonData filesData = data["files"];
			for (int j = 0; j < filesData.Count; ++j)
			{
				JsonData fileData = filesData[j];
				string filename = fileData.ToString();

				asset = Resources.Load<TextAsset>("Excel/" + filename);
				string[] excel_lines = GetLines(asset);
				for (int l = 1; l < excel_lines.Length; ++l)
				{
					string excel_line = excel_lines[l];

					Type excel_type = Type.GetType("excel_" + className);
					FieldInfo excelViewField = excel_type.BaseType.GetField("excelView");
					Type viewType = excelViewField.FieldType;
					object vd = System.Activator.CreateInstance(viewType);
					MethodInfo addMethod = viewType.GetMethod("Add");

					Type[] viewKVTypes = viewType.GetGenericArguments();
					//addMethod.Invoke(vd, 
					//excelViewField.SetValue (null, excelView);
				}
			}
		}
	}

	static string[] GetLines(TextAsset asset)
	{
		int index = 0;
		int count = 0;
		List<string> list = new List<string>();
		for (int i = 0; i < asset.bytes.Length; ++i)
		{
			byte b = asset.bytes[i];
			if (b == '\r' || b == '\n' && count > 0)
			{
				string s = Encoding.UTF8.GetString(asset.bytes, index, count);
				list.Add(s);
				count = 0;
				index = i + 1;
				continue;
			}
			++count;
		}
		return list.ToArray();
	}
}

public class ExcelBase<T>
{
	public static Dictionary<int, T> excelView = null;

	public static T Find(int id)
	{
		T excel = default(T);
		if (excelView.TryGetValue(id, out excel))
		{
			return excel;
		}
		return default(T);
	}
}