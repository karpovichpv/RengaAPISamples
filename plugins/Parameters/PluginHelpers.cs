using Renga;
using System.Text;

internal static class PluginHelpers
{
	public static string ShowPlateStylesParameters(IApplication app)
	{
		StringBuilder builder = new();
		builder.AppendLine("Plate style parameters:");
		IProject project = app.Project;
		IEntityCollection styles = project.PlateStyles;
		foreach (int id in styles.GetIds())
		{
			IEntity style = styles.GetById(id);

			if (style is IParameterContainer parameters)
			{
				builder.AppendLine($"{styles.GetById(id).Name}: {OutputParameters(parameters)}");
			}
		}

		return builder.ToString();
	}

	private static string OutputParameters(IParameterContainer parameters)
	{
		IGuidCollection ids = parameters.GetIds();
		StringBuilder builder = new();
		for (int i = 0; i < ids.Count; i++)
		{
			IParameter parameter = parameters.Get(ids.Get(i));
			builder.AppendLine($"{parameter.Definition.Text} {GetParameterValue(parameter)}");
		}

		return builder.ToString();
	}

	private static object? GetParameterValue(Renga.IParameter parameter)
	{
		return parameter.ValueType switch
		{
			ParameterValueType.ParameterValueType_Double => parameter.GetDoubleValue(),
			ParameterValueType.ParameterValueType_String => parameter.GetStringValue(),
			ParameterValueType.ParameterValueType_Int => parameter.GetIntValue(),
			ParameterValueType.ParameterValueType_Bool => parameter.GetBoolValue(),
			_ => null,
		};
	}
}