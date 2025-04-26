using Renga;
using System.Text;

namespace MaterialExport
{
	internal class Exporter
	{
		private readonly IProject _project;
		private readonly IMaterialManager _materialManager;
		private readonly ILayeredMaterialManager _layeredMaterialManager;
		private readonly IModelObjectCollection _modelObjects;

		public Exporter()
		{
			IApplication app = new Application();
			_project = app.Project;
			_materialManager = _project.MaterialManager;
			_layeredMaterialManager = _project.LayeredMaterialManager;
			_modelObjects = app.Project.Model.GetObjects();
		}

		internal string Export()
		{
			List<IObjectWithMaterial> ordynaryMaterialsObjects = [];
			List<IObjectWithLayeredMaterial> layeredMaterialsObjects = [];
			for (int i = 0; i < _modelObjects.Count; i++)
			{
				IModelObject obj = _modelObjects.GetByIndex(i);

				if (obj is IObjectWithMaterial ordynaryMaterialObject)
					ordynaryMaterialsObjects.Add(ordynaryMaterialObject);

				if (obj is IObjectWithLayeredMaterial layeredMaterialsObject)
					layeredMaterialsObjects.Add(layeredMaterialsObject);
			}

			string layeredMaterials = GetLayeredMaterials(layeredMaterialsObjects);
			string ordynaryMaterials = GetOrdynaryMaterials(ordynaryMaterialsObjects);

			return $"{ordynaryMaterials} \r\n {layeredMaterials}";
		}

		private string GetLayeredMaterials(List<IObjectWithLayeredMaterial> layeredMaterialsObjects)
		{
			var usedMaterialsIds
				= layeredMaterialsObjects
				.GroupBy(k => k.LayeredMaterialId);

			List<string> materialNames = [];
			foreach (var item in usedMaterialsIds)
			{
				if (item.Key != 1)
				{
					ILayeredMaterial entity = _layeredMaterialManager.GetLayeredMaterial(item.Key);
					if (entity is ILayeredMaterial layeredMaterial)
					{
						string layers = GetLayers(layeredMaterial);
						materialNames.Add($"{entity.Name}, слои:{layers}");
					}
				}
			}

			string names = string.Join("\r\n ", materialNames);

			return $"Многослойные материалы: {names}";
		}

		private static string GetLayers(ILayeredMaterial layeredMaterial)
		{
			StringBuilder builder = new();
			if (layeredMaterial.Layers is not null)
			{
				for (int i = 0; i < layeredMaterial.Layers.Count; i++)
				{
					IMaterialLayer layer = layeredMaterial.Layers.Get(i);
					builder.AppendLine($"Материал: {layer.Material.Name}; толщина: {layer.Thickness}");
				}
			}

			return builder.ToString();
		}

		private string GetOrdynaryMaterials(List<IObjectWithMaterial> ordynaryMaterialsObjects)
		{
			var usedMaterialsIds
				= ordynaryMaterialsObjects
				.GroupBy(k => k.MaterialId);

			List<string> materialNames = [];
			foreach (var item in usedMaterialsIds)
			{
				if (item.Key != 1)
					materialNames.Add(_materialManager.GetMaterial(item.Key).Name);
			}

			string names = string.Join("\r\n ", materialNames);

			return $"Однослойные материалы: {names}";
		}
	}
}