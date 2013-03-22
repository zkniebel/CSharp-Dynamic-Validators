using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Text;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace Website.Validation {
    /// <summary>
    /// Validates that the number of fields out of those specified that are not blank falls between the specified subset 
    /// minimum size and subset maximum size 
    /// </summary>
    public class RequiredSubsetOfFieldsValidator : BaseDynamicValidator {
        private List<string> controlIDs;
        private string _ControlToValidate = "";
        private int _SubsetMaxSize = -1;
        private int _SubsetMinSize = -1;

        /// <summary>
        /// A pipe-delimited list of control IDs (required)
        /// </summary>
        [DefaultValue("")]
        public new string ControlToValidate {
            get {
                return _ControlToValidate;
            }
            set {
                _ControlToValidate = value;
                controlIDs = _ControlToValidate.Split(new char[]{'|'}).Select(i => i.Trim()).ToList();
            }
        }
        /// <summary>
        /// The maximum number of fields that must be completed (optional - if not set, no maximum will be applied)
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
        /// The minimum number of fields that must be completed (required)
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
            if (result) {
                result =
                (controlIDs != null) &&
                (this.FindControl(ControlToValidate) != null) &&
                (controlIDs.Count >= SubsetMinSize) &&
                (SubsetMinSize >= 0) &&
                ((SubsetMaxSize >= SubsetMinSize) || (SubsetMaxSize < 0));
            }
            if (result && controlIDs.Any()) {
                foreach (string id in controlIDs) {
                    result = result && this.FindControl(id) != null;
                }
            }
            return result;
        }

        protected override bool EvaluateIsValid() {
            if (EvaluateIsEnabled()) {
                var filledFieldsCount = 0;
                foreach (string id in controlIDs) {
                    if (!string.IsNullOrEmpty(this.GetControlValidationValue(id))) {
                        filledFieldsCount++;
                    }
                }

                return ((filledFieldsCount <= SubsetMaxSize) || (SubsetMaxSize < 0)) &&
                    (filledFieldsCount >= SubsetMinSize);
            } else {
                return true;
            }
        }
    }
}