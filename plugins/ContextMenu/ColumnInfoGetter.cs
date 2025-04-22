using Renga;
using System.Text;

namespace ContextMenu
{
	internal class ColumnInfoGetter(IModelObject modelObject, IProject project)
	{
		private readonly IModelObject _modelObject = modelObject;
		private readonly IProject _project = project;

		public string Get()
		{
			if (_modelObject is not IColumnParams)
				return "Object isn't a column";

			StringBuilder builder = new();
			if (_modelObject is IColumnParams parameters)
			{
				AddParams(builder, parameters);
				AddStyleParams(builder, parameters);
			}

			return builder.ToString();
		}

		private void AddStyleParams(StringBuilder builder, IColumnParams parameters)
		{
			IColumnStyleManager columnStyleManager = _project.ColumnStyleManager;
			IColumnStyle style = columnStyleManager.GetColumnStyle(parameters.StyleId);

			builder.AppendLine($"Style Id:{style.Id}");
			builder.AppendLine($"Style Name:{style.Name}");
			builder.AppendLine($"Style Profile:{style.Profile}");
		}

		private static void AddParams(StringBuilder builder, IColumnParams parameters)
		{
			builder.AppendLine($"Height:{parameters.Height}");
			builder.AppendLine($"Position:{parameters.Position}");
			builder.AppendLine($"StyleId:{parameters.StyleId}");
			builder.AppendLine($"VerticalOffset:{parameters.VerticalOffset}");
			builder.AppendLine($"Placement:{parameters.GetProfilePlacement()}");
		}
	}
}
