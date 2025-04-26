using Renga;

namespace RebarQuantity
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
			exportObjects.ToolTip = "Rebar Quantity";
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
			IModelObject? column = GetSelectedObject();
			RebarQuantityGetter getter = new(column);
			string exportString = getter.Get();
			_ui?.ShowMessageBox(
				MessageIcon.MessageIcon_Info,
				"Rebar",
				exportString);
		}

		private IModelObject? GetSelectedObject()
		{
			IModel? model = null;
			if (_app is not null)
			{
				model = _app.Project.Model;
				_selection = _app.Selection;
			}

			IModelObjectCollection? modelObjects = null;
			if (model is not null)
				modelObjects = model.GetObjects();

			if (_selection is not null && modelObjects is not null)
			{
				int[] objects = (int[])_selection.GetSelectedObjects();
				if (objects.Length != 0)
				{
					object? currentId = objects.GetValue(0);
					if (currentId is int id)
					{
						IModelObject? obj = modelObjects?.GetById(id);
						return obj;
					}
				}
			}

			return null;
		}
	}
}
