using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;

namespace Website.Validation {
    /// <summary>
    /// A RegularExpressionValidator control that can be enabled/disabled by the same methods and properties as are provided 
    /// in the <seealso cref="Website.Validation.BaseDynamicValidator"/> class
    /// </summary>
    /// <remarks>Note that this class inherits the class <see cref="System.Web.UI.WebControls.RegularExpressionValidator"/>,
    /// which does not inherit <seealso cref="Website.Validation.BaseDynamicValidator"/>. This class does, however, 
    /// contain the same methods and properties that the <seealso cref="Website.Validation.BaseDynamicValidator"/> class 
    /// uses to make the validator dynamically enabled.
    /// </remarks>
    public class DynamicRegularExpressionValidator : RegularExpressionValidator {
        private string _ControlThatEnables = string.Empty;
        private string _ControlValueThatEnables = string.Empty;
        private bool _DynamicallyEnabled = true;

        /// <summary>
        /// Value that the control referenced by the ControlThatEnables property must have in order for the validator 
        /// to be enabled
        /// </summary>
        [DefaultValue("")]
        public string ControlValueThatEnables {
            get {
                return _ControlValueThatEnables;
            }
            set {
                _ControlValueThatEnables = value;
            }
        }
        /// <summary>
        /// ID of the control whose value determines whether or not the validator is enabled or disabled
        /// </summary>
        [TypeConverter(typeof(ValidatedControlConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string ControlThatEnables {
            get {
                return _ControlThatEnables;
            }
            set {
                _ControlThatEnables = value;
            }
        }
        /// <summary>
        /// Indicates whether or not the validator is enabled dynamically (default is true)
        /// </summary>
        [DefaultValue(true)]
        public bool DynamicallyEnabled {
            get {
                return _DynamicallyEnabled;
            }
            set {
                _DynamicallyEnabled = value;
            }
        }

        protected override bool ControlPropertiesValid() {
            return ((DynamicallyEnabled && (!string.IsNullOrEmpty(ControlThatEnables)) && (!string.IsNullOrEmpty(ControlValueThatEnables))) ||
                !DynamicallyEnabled) && base.ControlPropertiesValid();
        }

        protected override bool EvaluateIsValid() {
            bool isEnabled = EvaluateIsEnabled();

            if (isEnabled) {
                return base.EvaluateIsValid();
            } else {
                return true;
            }
        }

        /// <summary>
        /// Checks if the validator should be enabled or disabled and returns a flag that indicates the result
        /// </summary>
        /// <remarks>Note: This method does not enable or disable the validator - it merely returns a flag</remarks>
        /// <returns>True if the validator should be enabled, otherwise false</returns>
        protected bool EvaluateIsEnabled() {
            if (!ControlPropertiesValid()) {
                return true;
            }

            if (DynamicallyEnabled) {
                var isValidatorEnabled = false;
                var enablingControl = this.FindControl(ControlThatEnables);
                if (enablingControl != null) {
                    try {
                        if (enablingControl is CheckBox) {
                            isValidatorEnabled = ((CheckBox)enablingControl).Checked && bool.Parse(ControlValueThatEnables);
                        } else {
                            isValidatorEnabled = this.GetControlValidationValue(ControlThatEnables) == ControlValueThatEnables;
                        }
                    } catch {
                        isValidatorEnabled = false;
                    }
                }
                return isValidatorEnabled;
            } else {
                return true;
            }
        }
    }
}