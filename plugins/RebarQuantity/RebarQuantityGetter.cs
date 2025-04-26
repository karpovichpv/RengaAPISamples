using Renga;
using System.Text;

namespace RebarQuantity
{
	internal class RebarQuantityGetter
	{
		private readonly IModelObject? _object;
		private readonly IReinforcementUnitStyleManager? _reinforcementStyleManager;

		public RebarQuantityGetter(IModelObject? column)
		{
			_object = column;
			IApplication app = new Application();

			if (app is not null)
			{
				IProject project = app.Project;
				if (project is not null)
				{
					_reinforcementStyleManager = project.ReinforcementUnitStyleManager;
				}
			}

		}

		internal string Get()
		{
			StringBuilder builder = new();
			if (_object is not null)
			{
				if (_object is IObjectReinforcementModel reinforcementObject)
				{
					builder.Append(GetUsages(reinforcementObject));
				}


				return builder.ToString();
			}

			return "Не возможно посчить какие-либо характеристики для объекта";
		}

		private string GetUsages(IObjectReinforcementModel reinforcementObject)
		{
			StringBuilder builder = new();
			IRebarUsageCollection usages = reinforcementObject.GetRebarUsages();
			for (int i = 0; i < usages.Count; i++)
			{
				IRebarUsage current = usages.Get(i);
				IQuantityContainer quantities = current.GetQuantities();
				builder.AppendLine("-----");
				builder.AppendLine(GetStyleProperties(current));
				builder.AppendLine(GetLengthString(quantities));
				builder.AppendLine(GetMassString(quantities));
			}

			return builder.ToString();
		}

		private string GetStyleProperties(IRebarUsage current)
		{
			if (_reinforcementStyleManager is not null)
			{

				IRebarStyle rebarStyle = _reinforcementStyleManager.GetRebarStyle(current.StyleId);
				if (rebarStyle is not null)
				{
					return
						$"Diameter: {rebarStyle.Diameter:f2}; " +
						$"name: {rebarStyle.Name}; " +
						$"grade name: {rebarStyle.GradeName}; " +
						$"tensileStrength {rebarStyle.GradeTensileStrength}";
				}
			}
			return "Cannot obtain the rebar style parameters parameters";
		}

		private static string GetLengthString(IQuantityContainer quantities)
		{
			IQuantity length = quantities.Get(QuantityIds.TotalRebarLength);
			return length is not null
				? $"Length: {length.AsLength(LengthUnit.LengthUnit_Millimeters)} cm"
				: "Не возможно получить длину";
		}

		private static string GetMassString(IQuantityContainer quantities)
		{
			IQuantity value = quantities.Get(QuantityIds.TotalRebarMass);
			return value is not null
				? $"Mass: {value.AsMass(MassUnit.MassUnit_Kilograms)} kg"
				: "Не возможно получить массу";
		}
	}
}