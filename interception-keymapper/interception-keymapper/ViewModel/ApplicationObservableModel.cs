using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Windows.Input;
using InterceptionKeymapper.Helpers;


namespace InterceptionKeymapper.ViewModel
{
    public class ApplicationObservableModel : ObservableModelBase
    {
        private ObservableModelBase _observableModelBase;
        //first control that will be displayed
        private const string ENTRY_POINT = "listViewUserControl";
        private Dictionary<string, ObservableModelBase> ViewModels = new Dictionary<string, ObservableModelBase>();
        public ObservableModelBase CurrentViewModel
        {
            get { return _observableModelBase; }
            set
            {
                _observableModelBase = value;
                OnPropertyChanged("CurrentViewModel");
            }
        }
        public ApplicationObservableModel()
        {
            //Add every viewname to matching viewmodel

            ViewModels.Add("listViewUserControl", new ListViewViewModel());
            ViewModels.Add("showDevicesWindow", new ShowDevicesViewModel());
            ViewModels.Add("addDeviceWindow", new AddDeviceViewModel());
            
            UIStateManager.Instance.OnUiStateChanged += OnUIStateChanged;
            CurrentViewModel = ViewModels[ENTRY_POINT];
        }
        private void OnUIStateChanged(object sender, UiChangedEventArgs e)
        {
            CurrentViewModel = ViewModels[e.StateName];
        }

        #region Properties

        #endregion


    }
}



