using Renga;
using System.Text;

namespace ColumnMaterialQuantity
{
	internal class MaterialCalculator(IModelObject? column)
	{
		private readonly IModelObject? _column = column;

		internal string Calculate()
		{
			StringBuilder builder = new();
			if (_column is not null)
			{
				IQuantityContainer quantities = _column.GetQuantities();

				builder.AppendLine(GetAreaString(quantities));
				builder.AppendLine(GetCrossSectionAreaString(quantities));

				return builder.ToString();
			}

			return "Не возможно посчить какие-либо характеристики для объекта";
		}

		private static string GetAreaString(IQuantityContainer quantities)
		{
			IQuantity area = quantities.Get(QuantityIds.Area);
			return area is not null
				? $"Area {area.AsArea(AreaUnit.AreaUnit_Centimeters2)} cm2"
				: "Не возможно получить площадь";
		}

		private static string GetCrossSectionAreaString(IQuantityContainer quantities)
		{
			IQuantity area = quantities.Get(QuantityIds.NetCrossSectionArea);
			return area is not null
				? $"Площадь поперечного сечения {area.AsArea(AreaUnit.AreaUnit_Centimeters2)} см2"
				: "Не возможно получить площадь поперечного сечения";
		}
	}
}