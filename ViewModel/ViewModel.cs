using System;
using System.ComponentModel;
using VM.MVVM;
using SimpleDeviceCommunication;
using DeviceCommunication;
using System.Text.RegularExpressions;
using DeviceCommunication.CommunicationClasses;
using System.Collections.Generic;

namespace VM
{
    /// <summary>
    /// Implementation of ViewModel component. 
    /// 
    /// </summary>
    public class ViewModel : ViewModelBase
    {
        public ViewModel()
        {
            CreateHttpConnectionCommand = new ViewModelCommand(CreateHttpConnection, true);
            SetColorCommand = new ViewModelCommand(SetColor, true);
            SetEffectCommand = new ViewModelCommand(SetEffect, true);
            AvailableEffects = new List<EffectType>();
            foreach (EffectType enumValue in Enum.GetValues<EffectType>())
                AvailableEffects.Add(enumValue);
            RaisePropertyChanged(nameof(AvailableEffects));
            NewEffect = AvailableEffects[0];
        }

        #region Private ViewModel elements

        private DeviceCommunicationFasade _model = null;

        #endregion

        #region Public ViewModel elements

        public Action DeviceAddressEnteredCorrectly = null;
        public Action<string> ShowMessage = null;

        #endregion

        #region Public binding properties
        /// <summary>
        /// Path to the device property
        /// </summary>
        public string DevicePath
        {
            get => _devicePath;
            set
            {
                _devicePath = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// New color property used to set color in the device.
        /// Implements regex check to ensure a valid data is written.
        /// </summary>
        public string NewColor
        {
            get => _newColor;
            set
            {
                if (Regex.IsMatch(value, "^(?:[0-9a-fA-F]{2}|--){1}$|^(?:[0-9a-fA-F]{2}|--){4,5}$"))
                    _newColor = value;
                SetColorCommand.CanExecuteValue = true;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Color fade time property used to set color in the device.
        /// Only numbers can be written.
        /// </summary>
        public string ColorFade
        {
            get => _colorFade.ToString();
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _colorFade = 0;
                else if (uint.TryParse(value, out uint newValue) && newValue <= 3600000)
                    _colorFade = newValue;
                SetColorCommand.CanExecuteValue = true;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Selected new effect property used to set effect in the device.
        /// </summary>
        public EffectType NewEffect
        {
            get => _newEffect;
            set
            {
                _newEffect = value;
                SetEffectCommand.CanExecuteValue = true;
                RaisePropertyChanged();
            }
        }


        /// <summary>
        /// Effect fade time property used to set effect in the device.
        /// Only numbers can be written.
        /// </summary>
        public string EffectFade
        {
            get => _effectFade.ToString();
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _effectFade = 0;
                else if (uint.TryParse(value, out uint newValue) && newValue <= 3600000)
                    _effectFade = newValue;
                SetEffectCommand.CanExecuteValue = true;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        /// Effect step time property used to set effect in the device.
        /// Only numbers can be written.
        /// </summary>
        public string EffectStep
        {
            get => _effectStep.ToString();
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _effectStep = 0;
                else if (uint.TryParse(value, out uint newValue) && newValue <= 3600000)
                    _effectStep = newValue;
                SetEffectCommand.CanExecuteValue = true;
                RaisePropertyChanged();
            }
        }

        public string DeviceName
        {
            get => _deviceName;
            private set
            {
                _deviceName = value;
                RaisePropertyChanged();
            }
        }
        public string DeviceProduct
        {
            get => _deviceProduct;
            private set
            {
                _deviceProduct = value;
                RaisePropertyChanged();
            }
        }

        public string DeviceIp
        {
            get => _deviceIp;
            private set
            {
                _deviceIp = value;
                RaisePropertyChanged();
            }
        }

        public string CurrentColor
        {
            get => _currentColor;
            private set
            {
                _currentColor = value;
                RaisePropertyChanged();
            }
        }

        public EffectType CurrentEffect
        {
            get => _currentEffect;
            private set
            {
                _currentEffect = value;
                RaisePropertyChanged();
            }
        }

        public List<EffectType> AvailableEffects
        {
            get => _availableEffects;
            private set
            {
                _availableEffects = value;
                RaisePropertyChanged();
            }
        }

        // Bindable ViewModel actions commands 
        public ViewModelCommand CreateHttpConnectionCommand { get; private set; }
        public ViewModelCommand SetColorCommand { get; private set; }
        public ViewModelCommand SetEffectCommand { get; private set; }
        #endregion

        #region Private binding fields

        // user writable fields
        private string _devicePath = "";
        private string _newColor = "--";
        private uint _colorFade = 1000;
        private List<EffectType> _availableEffects;
        private EffectType _newEffect = EffectType.None;
        private uint _effectFade = 1000;
        private uint _effectStep = 1000;

        // presentation fields
        private string _deviceName;
        private string _deviceProduct;
        private string _deviceIp;
        private string _currentColor;
        private EffectType _currentEffect;

        #endregion

        #region ViewModel actions


        /// <summary>
        /// On create http connection command handler.
        /// Tries to create new http connection with device at <seealso cref="DevicePath"/>.
        /// </summary>
        private void CreateHttpConnection()
        {
            if(string.IsNullOrEmpty(DevicePath))
            {
                ShowMessage?.Invoke("Device path cannot be empty");
                return;
            }
            IDeviceConnection newDeviceCommunication = new DummyConnection();
            if(!newDeviceCommunication.Connect(DevicePath))
            {
                ShowMessage?.Invoke("Addres is not valid: " + DevicePath);
                return;
            }
            _model = new DeviceCommunicationFasade(newDeviceCommunication);
            _model.deviceInfoEvent += (name, product, ip) =>
            {
                DeviceName = name;
                DeviceProduct = product;
                DeviceIp = ip;
            };
            _model.newLightingStateEvent += (colorMode, effect, color) =>
            {
                CurrentColor = color;
                CurrentEffect = effect;
            };
            DeviceAddressEnteredCorrectly?.Invoke();
            _model.GetDeviceInfo();
            _model.GetLightingStatus();
        }

        /// <summary>
        /// On set color command handler.
        /// Invokes model <seealso cref="DeviceCommunicationFasade.SetColor(string, uint)"/> method if parameters are correct.
        /// </summary>
        private void SetColor()
        {
            if (!Regex.IsMatch(NewColor, "^(?:[0-9a-fA-F]{2}|--){1}$|^(?:[0-9a-fA-F]{2}|--){4,5}$"))
            {
                ShowMessage?.Invoke("New color value is not correct");
                return;
            }
            if(_colorFade > 0 && _colorFade < 25)
            {
                ShowMessage?.Invoke("Color fade value out of range. Acceptable: 0 or [25, 3600000]");
                return;
            }
            _model.SetColor(NewColor, _colorFade);
            SetColorCommand.CanExecuteValue = false;
        }

        /// <summary>
        /// On set effect command handler.
        /// Invokes model <seealso cref="DeviceCommunicationFasade.SetEffect(EffectType, uint, uint)"/> method if parameters are correct.
        /// </summary>
        private void SetEffect()
        {
            if (_effectFade > 0 && _effectFade < 25)
            {
                ShowMessage?.Invoke("Effect fade value out of range. Acceptable: 0 or [25, 3600000]");
                return;
            }
            if (_effectStep > 0 && _effectStep < 25)
            {
                ShowMessage?.Invoke("Effect step value out of range. Acceptable: 0 or [25, 3600000]");
                return;
            }
            _model.SetEffect(_newEffect, _effectFade, _effectStep);
            SetEffectCommand.CanExecuteValue = false;
        }

        #endregion
    }
}
