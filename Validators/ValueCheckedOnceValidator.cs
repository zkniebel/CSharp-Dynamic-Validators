using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.ComponentModel;

namespace Website.Validation {
    /// <summary>
    /// Validates across all CheckBoxList controls with the specified CssClass that there are no values checked more than once
    /// </summary>
    public class ValueCheckedOnceValidator : BaseDynamicValidator {
        private string _CssClassToValidate = "";
        private bool _ShowFailedTextAsError = false;
        private string _PrependToFailedText = "";

        /// <summary>
        /// CssClass of CheckBoxList controls to be validated
        /// </summary>
        [TypeConverter(typeof(ValidatedControlConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string CssClassToValidate {
            get {
                return _CssClassToValidate;
            }
            set {
                _CssClassToValidate = value;
            }
        }
        /// <summary>
        /// If set to true, a comma-space separated list of the Text values of all list items with duplicated values will be set as
        /// the validator's error message (overrides the ErrorMessage property)
        /// </summary>
        [DefaultValue(false)]
        public bool ShowFailedTextAsError {
            get {
                return _ShowFailedTextAsError;
            }
            set {
                _ShowFailedTextAsError = value;
            }
        } 
        /// <summary>
        /// Prepends the given text to the list of the Text values of all list items with duplicated values (only functions if the 
        /// ShowFailedTextAsError property is set to true)
        /// </summary>
        [DefaultValue("")]
        public string PrependToFailedText {
            get {
                return _PrependToFailedText;
            }
            set {
                _PrependToFailedText = value;
            }
        }

        protected override bool ControlPropertiesValid() {
            return (CssClassToValidate.Trim() != "") && base.ControlPropertiesValid();
        }

        protected override bool EvaluateIsValid() {
            if (!EvaluateIsEnabled()) {
                return true;
            }
            var cbItems = GetCheckBoxes(Page.Controls);
            var selected = cbItems.Where(i => i.Selected).ToList();
            var dups = GetDuplicateItemsByValue(selected);

            if (dups.Any()) {
                if (ShowFailedTextAsError == true) {
                    var sb = new StringBuilder(PrependToFailedText);
                    foreach (ListItem li in dups) {
                        sb.AppendFormat("{0}, ", li.Text);
                    }
                    sb.Remove(sb.Length - 2, 2);
                    ErrorMessage = sb.ToString();
                }
                return false;
            }
            return true;
        }

        /// <summary>
        /// Gets a list of the first occurrance of each ListItem whose value appears more than once in the given list
        /// </summary>
        /// <remarks>Although two ListItems may be different but have the same value, this method would only add the first
        /// of the two ListItems to the returned list.</remarks>
        /// <param name="listItems"></param>
        /// <returns></returns>
        private List<ListItem> GetDuplicateItemsByValue(List<ListItem> listItems) {
            var dups = new List<ListItem>(); 
            if (listItems.Count > 1) {
                var li = listItems.ElementAt(0);
                listItems.RemoveAt(0);

                if (listItems.RemoveAll(i => i.Value == li.Value) > 0) {
                    dups.Add(li);
                    dups.AddRange(GetDuplicateItemsByValue(listItems));
                } else {
                    dups = GetDuplicateItemsByValue(listItems);
                }
            }

            return dups;
        }

        /// <summary>
        /// Gets a list of all of the ListItems from all of the CheckBoxList controls that have the CssClass to be validated
        /// </summary>
        /// <param name="controlCollection">The collection of controls in which to search for CheckBoxLists</param>
        /// <returns></returns>
        private List<ListItem> GetCheckBoxes(ControlCollection controlCollection) {
            var cbItems = new List<ListItem>();

            foreach (Control control in controlCollection) {
                if (control is CheckBoxList) {
                    var cbl = ((CheckBoxList)control);
                    if (cbl.CssClass.Contains(CssClassToValidate)) {
                        cbItems.AddRange(GetListItems(cbl.Items));
                    }
                }

                if (control.HasControls()) {
                    cbItems.AddRange(GetCheckBoxes(control.Controls));
                }
            }
            return cbItems;
        }

        /// <summary>
        /// Returns a list of all of the ListItems in a ListItemCollection
        /// </summary>
        /// <param name="collection">The collection to get the ListItems from</param>
        /// <returns>A list of all ListItems in the given collection</returns>
        private List<ListItem> GetListItems(ListItemCollection collection) {
            var listItems = new List<ListItem>();
            foreach (ListItem li in collection) {
                listItems.Add(li);
            }
            return listItems;
        }

    }
}