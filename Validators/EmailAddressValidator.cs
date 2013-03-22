using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.ComponentModel;

namespace Website.Validation {
    /// <summary>
    /// Validates that the value of the control referenced by the ControlToValidate property is a properly formatted email address
    /// </summary>
    public class EmailAddressValidator : BaseDynamicValidator {
        private bool _EnsureDomainSuffix = true;
        /// <summary>
        /// Ensures that all addresses end with a domain suffix (e.g. '.com', '.gov', etc.)
        /// </summary>
        /// <remarks>
        /// Technically, email addresses may be valid even if they do not end with a domain suffix. However, this is
        /// very rarely the case, so the validator provides this regex to optionally ensure that a domain suffix is 
        /// present.
        /// </remarks>
        private static readonly Regex suffixRE = new Regex(@"(\w+)\.(\w+)");

        /// <summary>
        /// Indicates whether or not validator should ensure that email address to be validated ends with a domain
        /// suffix (e.g. '.com', '.gov', etc.)
        /// </summary>
        [DefaultValue(true)]
        public bool EnsureDomainSuffix {
            get {
                return _EnsureDomainSuffix;
            }
            set {
                _EnsureDomainSuffix = value;
            }
        }

        protected override bool ControlPropertiesValid() {
            return base.ControlPropertiesValid() && BaseValidatorControlPropertiesValid();
        }

        protected override bool EvaluateIsValid() {
            if (!EvaluateIsEnabled()) {
                return true;
            }

            var value = this.GetControlValidationValue(ControlToValidate);
            
            if (value == null) {
                return false;
            } else if (value.Trim() == "") {
                return true;
            }
            
            try {
                MailAddress ma = new MailAddress(value);
                return suffixRE.IsMatch(ma.Host) || !EnsureDomainSuffix;
            } catch {
                return false;
            }
        }
    }
}