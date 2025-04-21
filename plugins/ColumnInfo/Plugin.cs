using Renga;
using System.Text;

namespace ColumnInfo
{
	public class Plugin : IPlugin
	{
		private readonly List<ActionEventSource> _actionEventSources = [];
		private IApplication? _app;

		public bool Initialize(string pluginFolder)
		{
			IApplication app = new Application();
			_app = app;
			IUI ui = app.UI;

			IUIPanelExtension panelExtension = ui.CreateUIPanelExtension();

			panelExtension.AddToolButton(CreateAction(ui));
			ui.AddExtensionToPrimaryPanel(panelExtension);
			ui.AddExtensionToActionsPanel(panelExtension, ViewType.ViewType_View3D);

			return true;
		}


		public void Stop()
		{
			foreach (ActionEventSource eventSource in _actionEventSources)
				eventSource.Dispose();

			_actionEventSources.Clear();
		}

		private IAction CreateAction(IUI ui)
		{
			IAction action = ui.CreateAction();

			ActionEventSource eventSource = new(action);
			eventSource.Triggered += (obj, args) =>
			{
				string columnInfo = GetColumnInfo();
				ui.ShowMessageBox(MessageIcon.MessageIcon_Info, "sample plugin", columnInfo);
			};

			_actionEventSources.Add(eventSource);
			return action;
		}

		private string GetColumnInfo()
		{
			IProject? project = null;
			ISelection? selection = null;
			if (_app is not null)
			{
				project = _app.Project;
				selection = _app.Selection;
			}

			if (project is null)
				return $"Project is null";

			if (selection is null)
			{
				return "Nothing was selected";
			}

			Array array = selection.GetSelectedObjects();
			StringBuilder builder = new();
			foreach (object obj in array)
			{
				if (obj is int id)
				{
					IModel model = project.Model;
					IModelObject modelObject = model.GetObjects().GetById(id);

					ColumnInfoGetter getter = new(modelObject);
					return getter.Get();
				}
				else
					builder.Append($"obj:{obj.GetType()}");
			}

			return builder.ToString();
		}
	}
}
