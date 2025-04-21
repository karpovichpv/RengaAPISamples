using Renga;
using System.Text;

namespace ColumnInfo
{
	internal class ColumnInfoGetter(IModelObject modelObject)
	{
		private readonly IModelObject _modelObject = modelObject;

		public string Get()
		{
			StringBuilder builder = new();

			if (_modelObject is IColumnParams parameters)
			{
				builder.AppendLine($"Height:{parameters.Height}");
				builder.AppendLine($"Position:{parameters.Position}");
				builder.AppendLine($"StyleId:{parameters.StyleId}");
				builder.AppendLine($"VerticalOffset:{parameters.VerticalOffset}");
				builder.AppendLine($"Placement:{parameters.GetProfilePlacement()}");
			}
			return builder.ToString();
		}
	}
}
