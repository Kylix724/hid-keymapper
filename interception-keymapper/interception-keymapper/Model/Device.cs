using InterceptionKeymapper.Helpers;

namespace InterceptionKeymapper.Model
{
	public class Device : ObservableModelBase
	{
		private string _hwid;
		private string _name;
		private bool _active;

		public Device(string hwid, string name)
		{
			Hwid = hwid;
			Name = name;
			_active = true;
		}

		public string Hwid
		{
			get => _hwid;
			set
			{
				_hwid = value;
				OnPropertyChanged("Hwid");
			}
		}

		public string Name
		{
			get => _name;
			set
			{
				_name = value;
				OnPropertyChanged("Name");
			}
		}

		public bool Active
		{
			get => _active;
			set
			{
				_active = value;
				OnPropertyChanged("Active");
			}
		}

		public override string ToString()
		{
			return $"{Name}({Hwid})";
		}
	}
}
