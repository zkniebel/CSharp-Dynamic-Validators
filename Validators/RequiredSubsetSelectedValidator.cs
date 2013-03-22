using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.ComponentModel;

namespace Website.Validation {
    /// <summary>
    /// Validates that the number of selected ListItems in the control being validated falls within the given maximum and minimum 
    /// sizes. This validator is intended for use with all ListControls (CheckBoxList, RadioButtonList, ListBox, etc.).
    /// </summary>
    public class RequiredSubsetSelectedValidator : BaseDynamicValidator {
        private int _SubsetMaxSize = -1;
        private int _SubsetMinSize = -1;

        /// <summary>
        /// The maximum number of ListItems that must be selected (optional - if not set, no maximum will be applied)
        /// </summary>
        [DefaultValue(-1)]
        public int SubsetMaxSize {
            get {
                return _SubsetMaxSize;
            }
            set {
                _SubsetMaxSize = value;
            }
        }
        /// <summary>
        /// The minimum number of ListItems that must be selected (required)
        /// </summary>
        [DefaultValue(-1)]
        public int SubsetMinSize {
            get {
                return _SubsetMinSize;
            }
            set {
                _SubsetMinSize = value;
            }
        }

        protected override bool ControlPropertiesValid() {
            var result = base.ControlPropertiesValid();
            if (!result) {
                return false;
            }

            var ctv = this.FindControl(ControlToValidate);
            if ((ctv != null) && (ctv is ListControl)) {
                var ctvList = (ListControl)ctv;
                result =
                    (ctvList.Items.Count >= SubsetMinSize) &&
                    (SubsetMinSize >= 0) &&
                    ((SubsetMaxSize >= SubsetMinSize) || (SubsetMaxSize < 0));
                return result;
            } else {
                return false;
            }
        }

        protected override bool EvaluateIsValid() {
            if (EvaluateIsEnabled()) {
                var selectedCount = 0;
                var ctvList = this.FindControl(ControlToValidate) as ListControl;
                foreach (ListItem li in ctvList.Items) {
                    if (li.Selected) {
                        selectedCount++;
                    }
                }

                return ((selectedCount <= SubsetMaxSize) || (SubsetMaxSize < 0)) &&
                    (selectedCount >= SubsetMinSize);
            } else {
                return true;
            }
        }
    }
}