using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MyPadelDesktopApp.Helpers
{
    public class FieldValidations
    {
        public static (bool, string) IsEmailValid(string email)
        {
            try
            {
                var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
                if (string.IsNullOrWhiteSpace(email))
                    return (true, "Email is required");

                if (!Regex.IsMatch(email, emailRegex))
                    return (true, "Invalid email");
                else
                    return (false, string.Empty);
            }
            catch
            {
                return (true, "Email is required");
            }
        }
        public static (bool, string) IsFieldNotEmpty(string Text, string ErrorMessage)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Text))
                    return (true, ErrorMessage);
                else
                    return (false, string.Empty);
            }
            catch
            {
                return (true, ErrorMessage);
            }
        }
        public static (bool, string) IsItalianPhoneNumberValid(string phoneNumber)
        {
            try
            {
                phoneNumber = phoneNumber.Replace(" ", "").Replace("-", "");

                var phoneRegex = @"^(\+39|0039)?[0-9]{2,4}[0-9]{6,8}$|^(\+39|0039)?3[0-9]{8,9}$";

                if (string.IsNullOrWhiteSpace(phoneNumber))
                    return (true, "Phone number is required");

                if (!Regex.IsMatch(phoneNumber, phoneRegex))
                    return (true, "Invalid Italian phone number format. Example of valid formats:\n- Landline: +39 06 1234567 (Rome)\n- Mobile: +39 345 1234567");

                return (false, string.Empty);
            }
            catch (Exception)
            {
            }
            return (true, "Invalid Italian phone number format. Example of valid formats:\n- Landline: +39 06 1234567 (Rome)\n- Mobile: +39 345 1234567");
        }
        public static (bool, string) IsFitCardExpiryDateValid(DateTime? date, string errorMessage)
        {
            try
            {
                if (date < DateTime.Today)
                    return (true, "Date must be in the future");

                return (false, string.Empty);
            }
            catch
            {
                return (true, "Invalid FIT card expiry date");
            }
        }
        public static (bool, string) IsNumericAndPositive(string value, string errorMessage)
        {
            try
            {
                return int.TryParse(value, out int result) && result > 0 ? (false, string.Empty) : (true, errorMessage);
            }
            catch { }
            return (false, errorMessage);
        }
    }
}
