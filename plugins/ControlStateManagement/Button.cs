using Renga;

namespace ControlStateManagement
{
	internal class Button
	{
		private readonly IUI _ui;
		private readonly string _tooltip;
		private readonly ActionEventSource _eventSource;

		public Button(IUI ui, string tooltip)
		{
			_ui = ui;
			_tooltip = tooltip;
			Action = ui.CreateAction();
			Action.ToolTip = tooltip;
			_eventSource = new ActionEventSource(Action);
			_eventSource.Triggered += OnActionEventsTriggered;
		}

		public IAction Action { get; }

		private void OnActionEventsTriggered(object? sender, EventArgs e)
		{
			_ui.ShowMessageBox(
				MessageIcon.MessageIcon_Info,
				"ControlStateManagementPlugin",
				"First button was pressed");
		}

		public void AddToPanel(IUIPanelExtension panel)
		{
			panel.AddToolButton(Action);
		}
	}
}
