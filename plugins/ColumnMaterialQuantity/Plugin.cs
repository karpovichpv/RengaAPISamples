using Renga;

namespace ColumnMaterialQuantity
{
	public class Plugin : IPlugin
	{
		private Application? _app;
		private ISelection? _selection;
		private IUI? _ui;
		private ActionEventSource? _eventSource;

		public bool Initialize(string pluginFolder)
		{
			_app = new Application();
			_ui = _app.UI;

			IUIPanelExtension panelExtension = _ui.CreateUIPanelExtension();
			IAction exportObjects = _ui.CreateAction();
			exportObjects.ToolTip = "Column Material Quantity";
			_eventSource = new ActionEventSource(exportObjects);
			_eventSource.Triggered += OnExportObjectsButtonClicked;
			panelExtension.AddToolButton(exportObjects);

			_ui.AddExtensionToPrimaryPanel(panelExtension);

			return true;
		}


		public void Stop()
		{
			_eventSource?.Dispose();
		}

		private void OnExportObjectsButtonClicked(object? sender, EventArgs e)
		{
			IModelObject? column = GetSelectedColumn();
			MaterialCalculator exporter = new(column);
			string exportString = exporter.Calculate();
			_ui?.ShowMessageBox(
				MessageIcon.MessageIcon_Info,
				"Materials was exported",
				exportString);
		}

		private IModelObject? GetSelectedColumn()
		{
			IModel? model = null;
			if (_app is not null)
			{
				model = _app.Project.Model;
				_selection = _app.Selection;
			}

			IModelObjectCollection? modelObjects = null;
			if (model is not null)
			{
				modelObjects = model.GetObjects();
			}

			if (_selection is not null && modelObjects is not null)
			{
				Array objects = _selection.GetSelectedObjects();
				for (int i = 0; i < objects.Length; i++)
				{
					object? currentId = objects.GetValue(i);
					if (currentId is int id)
					{
						IModelObject? obj = modelObjects?.GetById(id);
						if (obj is IColumnParams)
							return obj;
					}
				}
			}

			return null;
		}
	}
}
