
using Renga;
using System.Text;

namespace ExportGeometry
{
	internal class Exporter
	{
		private readonly ISelection _selection;
		private readonly IDataExporter _dataExporter;

		public Exporter()
		{
			IApplication app = new Application();
			_selection = app.Selection;
			_dataExporter = app.Project.DataExporter;
		}

		internal string Export()
		{
			IExportedObject3DCollection objects = _dataExporter.GetObjects3D();
			StringBuilder builder = new();
			Array selectedObjects = _selection.GetSelectedObjects();

			object? firstId = selectedObjects.GetValue(0);

			if (firstId is not null)
			{
				int id = (int)firstId;

				IExportedObject3D obj = objects.Get(id);
				builder.AppendLine($"ObjectType: {obj.ModelObjectType}");
				for (int j = 0; j < obj.MeshCount; j++)
				{
					IMesh mesh = obj.GetMesh(j);
					builder.AppendLine($"MeshGuid: {mesh.MeshType}");
					for (int k = 0; k < mesh.GridCount; k++)
					{
						IGrid grid = mesh.GetGrid(k);
						builder.Append($"GridType {grid.GridType}|");
						builder.Append($" NormalCount {grid.NormalCount}");
					}
				}
			}

			return builder.ToString();
		}
	}
}