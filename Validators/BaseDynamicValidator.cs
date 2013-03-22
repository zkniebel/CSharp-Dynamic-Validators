using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;

namespace Website.Validation {
    /// <summary>
    /// Abstract class for a validator that is enabled dynamically, based on the value of another control
    /// </summary>
    /// <remarks>Note that this classmerely provides the necessary methods and properties for enabling/disabling a
    /// validator, and does not directly enable or disable any controls (including itself).</remarks>
    /// <example>
    /// (1) The ControlThatEnables is set to a TextBox control. If ControlValueThatEnables is set to "Foo", then the 
    /// validator will only run if Text property of the TextBox is "Foo". 
    /// (2) The ControlThatEnables is set to a ListControl. If ControlValueThatEnables is set to "Bar", then the 
    /// validator will only run the if the SelectedValue property of the ListControl is "Bar".
    /// (3) The ControlThatEnables is set to a CheckBox control. If ControlValueThatEnables is set to "true", then the 
    /// validator will only run if the CheckBox is checked. Likewise, if ControlValueThatEnables is set to "false", 
    /// the validator will only run if the CheckBox is not checked. 
    /// </example>
    public abstract class BaseDynamicValidator : BaseValidator {
        private string _ControlThatEnables = "";
        private string _ControlValueThatEnables = "";
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
            return 
                (DynamicallyEnabled && (!string.IsNullOrEmpty(ControlThatEnables)) && (!string.IsNullOrEmpty(ControlValueThatEnables))) ||
                !DynamicallyEnabled;
        }

        /// <summary>
        /// Calls the ControlPropertiesValid method in the BaseValidator class
        /// </summary>
        protected bool BaseValidatorControlPropertiesValid() {
             return base.ControlPropertiesValid();
        }

        protected override abstract bool EvaluateIsValid();

        /// <summary>
        /// Checks if the validator should be enabled or disabled and returns a flag that indicates the result
        /// </summary>
        /// <remarks>Note: This method does not enable or disable the validator - it merely returns a flag</remarks>
        /// <returns>True if the validator should be enabled, otherwise false</returns>
        protected bool EvaluateIsEnabled() {
            /** NOTE: this line is an implementation decision - should validator be enabled or disabled if the dynamic properties are 
             *  ignored or invalid? */
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