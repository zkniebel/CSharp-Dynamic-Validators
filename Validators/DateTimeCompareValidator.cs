using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.ComponentModel;
using System.Web.UI;
using System.Text;
using System.Globalization;

namespace Website.Validation {
    /// <summary>
    /// Validator to compare DateTime objects (both date and time), optionally parsed from multiple controls
    /// </summary>
    public class DateTimeCompareValidator : BaseDynamicValidator {
        private string _YearControlToValidate = string.Empty;
        private string _MonthControlToValidate = string.Empty;
        private string _DayControlToValidate = string.Empty;
        private string _HourControlToValidate = string.Empty;
        private string _MinutesControlToValidate = string.Empty;
        private string _SecondsControlToValidate = string.Empty;
        private string _DayNightControlToValidate = string.Empty;

        private string _YearControlToCompare = string.Empty;
        private string _MonthControlToCompare = string.Empty;
        private string _DayControlToCompare = string.Empty;
        private string _HourControlToCompare = string.Empty;
        private string _MinutesControlToCompare = string.Empty;
        private string _SecondsControlToCompare = string.Empty;
        private string _DayNightControlToCompare = string.Empty;

        private DateTimeCompareType _CompareType = DateTimeCompareType.Both;
        private ValidationCompareOperator _Operator;

        private string _ControlToCompare = string.Empty;
        private string _ValueToCompare = string.Empty;


        #region Public Properties
        /// <summary>
        /// Control to compare against (leave blank if comparing against DateTime parsed from multiple controls)
        /// </summary>
        [TypeConverter(typeof(ValidatedControlConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string ControlToCompare {
            get {
                return _ControlToCompare;
            }
            set {
                _ControlToCompare = value;
            }
        }
        /// <summary>
        /// Value to compare against (leave blank if comparing against DateTime parsed from multiple controls) 
        /// </summary>
        [DefaultValue("")]
        public string ValueToCompare {
            get {
                return _ValueToCompare;
            }
            set {
                _ValueToCompare = value;
            }
        }
        /// <summary>
        /// Year control to validate (leave blank if not validating years)
        /// </summary>
        [TypeConverter(typeof(ValidatedControlConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string YearControlToValidate {
            get {
                return _YearControlToValidate;
            }
            set {
                _YearControlToValidate = value;
            }
        }
        /// <summary>
        /// Month control to validate (leave blank if not validating months)
        /// </summary>
        [TypeConverter(typeof(ValidatedControlConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string MonthControlToValidate {
            get {
                return _MonthControlToValidate;
            }
            set {
                _MonthControlToValidate = value;
            }
        }
        /// <summary>
        /// Day control to validate (leave blank if not validating days)
        /// </summary>
        [TypeConverter(typeof(ValidatedControlConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string DayControlToValidate {
            get {
                return _DayControlToValidate;
            }
            set {
                _DayControlToValidate = value;
            }
        }
        /// <summary>
        /// Hour control to validate (leave blank if not validating hours)
        /// </summary>
        [TypeConverter(typeof(ValidatedControlConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string HourControlToValidate {
            get {
                return _HourControlToValidate;
            }
            set {
                _HourControlToValidate = value;
            }
        }
        /// <summary>
        /// Minute control to validate (leave blank if not validating minutes)
        /// </summary>
        [TypeConverter(typeof(ValidatedControlConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string MinutesControlToValidate {
            get {
                return _MinutesControlToValidate;
            }
            set {
                _MinutesControlToValidate = value;
            }
        }
        /// <summary>
        /// Seconds control to validate (leave blank if not validating seconds)
        /// </summary>
        [TypeConverter(typeof(ValidatedControlConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string SecondsControlToValidate {
            get {
                return _SecondsControlToValidate;
            }
            set {
                _SecondsControlToValidate = value;
            }
        }
        /// <summary>
        /// Day/Night (period) control to validate (leave blank if not validating Day/Night)
        /// </summary>
        [TypeConverter(typeof(ValidatedControlConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string DayNightControlToValidate {
            get {
                return _DayNightControlToValidate;
            }
            set {
                _DayNightControlToValidate = value;
            }
        }
        /// <summary>
        /// Year control to compare (leave blank if not validating years)
        /// </summary>
        [TypeConverter(typeof(ValidatedControlConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string YearControlToCompare {
            get {
                return _YearControlToCompare;
            }
            set {
                _YearControlToCompare = value;
            }
        }
        /// <summary>
        /// Month control to compare (leave blank if not validating months)
        /// </summary>
        [TypeConverter(typeof(ValidatedControlConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string MonthControlToCompare {
            get {
                return _MonthControlToCompare;
            }
            set {
                _MonthControlToCompare = value;
            }
        }
        /// <summary>
        /// Day control to compare (leave blank if not validating days)
        /// </summary>
        [TypeConverter(typeof(ValidatedControlConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string DayControlToCompare {
            get {
                return _DayControlToCompare;
            }
            set {
                _DayControlToCompare = value;
            }
        }
        /// <summary>
        /// Hour control to compare (leave blank if not validating hours)
        /// </summary>
        [TypeConverter(typeof(ValidatedControlConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string HourControlToCompare {
            get {
                return _HourControlToCompare;
            }
            set {
                _HourControlToCompare = value;
            }
        }
        /// <summary>
        /// Minute control to compare (leave blank if not validating minutes)
        /// </summary>
        [TypeConverter(typeof(ValidatedControlConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string MinutesControlToCompare {
            get {
                return _MinutesControlToCompare;
            }
            set {
                _MinutesControlToCompare = value;
            }
        }
        /// <summary>
        /// Seconds control to compare (leave blank if not validating seconds)
        /// </summary>
        [TypeConverter(typeof(ValidatedControlConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string SecondsControlToCompare {
            get {
                return _SecondsControlToCompare;
            }
            set {
                _SecondsControlToCompare = value;
            }
        }
        /// <summary>
        /// Day/Night (period) control to compare (leave blank if not validating Day/Night)
        /// </summary>
        [TypeConverter(typeof(ValidatedControlConverter))]
        [IDReferenceProperty]
        [DefaultValue("")]
        public string DayNightControlToCompare {
            get {
                return _DayNightControlToCompare;
            }
            set {
                _DayNightControlToCompare = value;
            }
        }
        /// <summary>
        /// Indicates whether or not only the date, time, or both is/are to be compared (default is both)
        /// </summary>
        [DefaultValue(DateTimeCompareType.Both)]
        public DateTimeCompareType CompareType {
            get {
                return _CompareType;
            }
            set {
                _CompareType = value;
            }
        }
        /// <summary>
        /// Sets the operator to be used for the comparison (required)
        /// </summary>
        public ValidationCompareOperator Operator {
            get {
                return _Operator;
            }
            set {
                _Operator = value;
            }
        }
        #endregion Public Properties

        protected override bool ControlPropertiesValid() {
            try {
                GetDateTimeToValidate();
                GetDateTimeToCompare();
            } catch {
                return false;
            }
            return (Operator != null) && base.ControlPropertiesValid();
        }

        protected override bool EvaluateIsValid() {
            if (base.EvaluateIsEnabled()) {
                var toValidate = GetDateTimeToValidate();
                var toCompare = GetDateTimeToCompare();
                int compareResult;
                if (CompareType == DateTimeCompareType.Time) {
                    compareResult = TimeSpan.Compare(toValidate.TimeOfDay, toCompare.TimeOfDay);
                } else if (CompareType == DateTimeCompareType.Date) {
                    toValidate = toValidate.Date;
                    toCompare = toCompare.Date;
                }
                compareResult = DateTime.Compare(toValidate, toCompare);

                return CheckAgainstOperator(compareResult);
            }

            return true;
        }

        /// <summary>
        /// Checks the result of the comparison against the Operator and returns a flag indicating whether or not
        /// the comparison operation returns true or false
        /// </summary>
        /// <param name="compareResult">int result of the date comparison</param>
        /// <returns>A bool indicating whether or not the comparison operation returns true or false</returns>
        protected bool CheckAgainstOperator(int compareResult) {
            switch (Operator) {
                case ValidationCompareOperator.DataTypeCheck:
                    return true;
                case ValidationCompareOperator.Equal:
                    return compareResult == 0;
                case ValidationCompareOperator.GreaterThan:
                    return compareResult > 0;
                case ValidationCompareOperator.GreaterThanEqual:
                    return compareResult >= 0;
                case ValidationCompareOperator.LessThan:
                    return compareResult < 0;
                case ValidationCompareOperator.LessThanEqual:
                    return compareResult <= 0;
                case ValidationCompareOperator.NotEqual:
                    return compareResult != 0;
                default:
                    throw new Exception("Invalid ValidationCompareOperator applied to DateTimeCompareValidator");
            }
        } 

        /// <summary>
        /// Parses the DateTime object to be validated
        /// </summary>
        /// <returns>The DateTime object to be validated</returns>
        private DateTime GetDateTimeToValidate() {
            var hasControlsToValidate = 
                (ControlToValidate != string.Empty) ||
                ((MonthControlToValidate != string.Empty) ||
                 (DayControlToValidate != string.Empty) ||
                 (YearControlToValidate != string.Empty) ||
                 (HourControlToValidate != string.Empty) ||
                 (MinutesControlToValidate != string.Empty) ||
                 (SecondsControlToValidate != string.Empty) ||
                 (DayNightControlToValidate != string.Empty));
            if (hasControlsToValidate) {
                if (ControlToValidate == string.Empty) {
                    var toValidate = GetDateString(
                                MonthControlToValidate,
                                DayControlToValidate,
                                YearControlToValidate,
                                HourControlToValidate,
                                MinutesControlToValidate,
                                SecondsControlToValidate,
                                DayNightControlToValidate
                            );
                    return DateTime.Parse(toValidate, CultureInfo.InvariantCulture);
                } else {
                    return DateTime.Parse(GetControlValidationValue(ControlToValidate), CultureInfo.InvariantCulture);
                }
            } else {
                throw new Exception("Datetime validator must have at least one control to validate");
            }
        }

        /// <summary>
        /// Parses the DateTime object to be compared
        /// </summary>
        /// <returns>The DateTime object to be compared</returns>
        private DateTime GetDateTimeToCompare() {
            if (ControlToCompare != string.Empty) {
                return DateTime.Parse(GetControlValidationValue(ControlToCompare), CultureInfo.InvariantCulture);
            } else if (ValueToCompare != string.Empty) {
                return DateTime.Parse(ValueToCompare, CultureInfo.InvariantCulture);
            } else {
                var toCompare = GetDateString(
                        MonthControlToCompare,
                        DayControlToCompare,
                        YearControlToCompare,
                        HourControlToCompare,
                        MinutesControlToCompare,
                        SecondsControlToCompare,
                        DayNightControlToCompare
                    );
                return DateTime.Parse(toCompare, CultureInfo.InvariantCulture);
            }
        }

        /// <summary>
        /// Returns a formatted date string ("month/day/year hour:minute daynight") from values of the controls with the 
        /// given IDs
        /// </summary>
        /// <param name="monthID">ID of the control containing the value of the Month</param>
        /// <param name="dayID">ID of the control containing the value of the Day</param>
        /// <param name="yearID">ID of the control containing the value of the Year</param>
        /// <param name="hourID">ID of the control containing the value of the Hour</param>
        /// <param name="minutesID">ID of the control containing the value of the Minute</param>
        /// <param name="secondsID">ID of the control containing the value of the Second</param>
        /// <param name="dayNightID">ID of the control containing the value of the DayNight (AM/PM)</param>
        /// <returns></returns>
        private string GetDateString(string monthID, string dayID, string yearID,
            string hourID, string minutesID, string secondsID, string dayNightID) {

            var date = new StringBuilder();
            if (monthID == string.Empty) {
                date.Append("01");
            } else {
                date.Append(this.GetControlValidationValue(monthID));
            }
            if (dayID == string.Empty) {
                date.Append("/01");
            } else {
                date.AppendFormat("/{0}", this.GetControlValidationValue(dayID));
            }
            if (yearID == string.Empty) {
                date.Append("/0001");
            } else {
                date.AppendFormat("/{0}", this.GetControlValidationValue(yearID));
            }
            if (hourID == string.Empty) {
                date.Append(" 12");
            } else {
                date.AppendFormat(" {0}", this.GetControlValidationValue(hourID));
            }
            if (minutesID == string.Empty) {
                date.Append(":00");
            } else {
                date.AppendFormat(":{0}", this.GetControlValidationValue(minutesID));
            }
            if (secondsID == string.Empty) {
                date.Append(":00");
            } else {
                date.AppendFormat(":{0}", this.GetControlValidationValue(secondsID));
            }
            if (dayNightID == string.Empty) {
                date.Append(" am");
            } else {
                date.AppendFormat(" {0}", this.GetControlValidationValue(dayNightID));
            }

            return date.ToString();
        }
    }

    public enum DateTimeCompareType {
        Date = 0,
        Time = 1,
        Both = 2
    }
}