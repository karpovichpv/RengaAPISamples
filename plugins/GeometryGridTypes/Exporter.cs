using Renga;
using System.Text;

namespace GeometryGridTypes
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
					string meshType = GetMeshType(mesh);
					builder.AppendLine($"type:{meshType}");
				}
			}

			return builder.ToString();
		}

		private static string GetMeshType(IMesh mesh)
		{
			Guid meshType = mesh.MeshType;
			if (meshType == MeshTypes.Undefined)
				return nameof(MeshTypes.Undefined);
			else if (meshType == MeshTypes.DoorLining)
				return nameof(MeshTypes.DoorLining);
			else if (meshType == MeshTypes.DoorPanel)
				return nameof(MeshTypes.DoorPanel);
			else if (meshType == MeshTypes.DoorPlatband)
				return nameof(MeshTypes.DoorPlatband);
			else if (meshType == MeshTypes.DoorReveal)
				return nameof(MeshTypes.DoorReveal);
			else if (meshType == MeshTypes.DoorThreshold)
				return nameof(MeshTypes.DoorThreshold);
			else if (meshType == MeshTypes.DoorTransom)
				return nameof(MeshTypes.DoorTransom);
			else if (meshType == MeshTypes.WindowOutwardSill)
				return nameof(MeshTypes.WindowOutwardSill);
			else if (meshType == MeshTypes.WindowPanel)
				return nameof(MeshTypes.WindowPanel);
			else if (meshType == MeshTypes.WindowReveal)
				return nameof(MeshTypes.WindowReveal);
			else if (meshType == MeshTypes.WindowSill)
				return nameof(MeshTypes.WindowSill);

			throw new Exception("Cannot defined a type");
		}
	}
}