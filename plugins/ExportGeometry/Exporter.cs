
using Renga;
using System.Text;

namespace ExportGeometry
{
	internal class Exporter
	{
		private readonly IDataExporter _dataExporter;

		public Exporter()
		{
			IApplication app = new Application();
			_dataExporter = app.Project.DataExporter;
		}

		internal string Export()
		{
			IExportedObject3DCollection objects = _dataExporter.GetObjects3D();
			StringBuilder builder = new();

			for (int i = 0; i < objects.Count; i++)
			{
				IExportedObject3D obj = objects.Get(i);
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