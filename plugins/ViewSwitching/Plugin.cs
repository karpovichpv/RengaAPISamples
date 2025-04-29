using Renga;

namespace ViewSwitching
{
	public class Plugin : IPlugin
	{
		private readonly List<ActionEventSource> _eventSources = [];

		private IApplication? _app;

		public bool Initialize(string pluginFolder)
		{
			IApplication app = new Application();
			_app = app;
			IUI ui = app.UI;

			IUIPanelExtension extension = ui.CreateUIPanelExtension();
			extension.AddToolButton(CreateMainViewAction(ui));
			extension.AddToolButton(CreateAssemblyViewAction(ui));
			ui.AddExtensionToPrimaryPanel(extension);

			return true;
		}

		public void Stop()
		{
			foreach (ActionEventSource eventSource in _eventSources)
				eventSource.Dispose();

			_eventSources.Clear();
		}

		private IAction CreateMainViewAction(IUI ui)
		{
			IAction action = ui.CreateAction();
			action.DisplayName = "MainActionView";

			var events = new ActionEventSource(action);
			events.Triggered += (sender, arguments) =>
			{
				if (_app is not null)
				{
					int buildingId = _app.Project.BuildingInfo.Id;
					_app.OpenViewByEntity(buildingId);
				}
			};

			_eventSources.Add(events);

			return action;
		}

		private IAction CreateAssemblyViewAction(IUI ui)
		{
			IAction action = ui.CreateAction();
			action.DisplayName = "AssemblyViewAction";

			var events = new ActionEventSource(action);
			events.Triggered += (sender, arguments) =>
			{
				if (_app is not null)
				{
					IEntityCollection assemblies = _app.Project.Assemblies;
					if (assemblies.Count == 0)
						return;

					List<int> assemblyIds = [];
					for (int i = 0; i < assemblies.Count; i++)
					{
						IEntity assembly = assemblies.GetByIndex(i);
						assemblyIds.Add(assembly.Id);
					}

					List<int> groupedIds = assemblyIds.Distinct().ToList();
					_app.OpenViewByEntity(groupedIds.First());
				}
			};

			_eventSources.Add(events);

			return action;
		}
	}
}