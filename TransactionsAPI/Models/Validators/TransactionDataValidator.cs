using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using TransactionsAPI.Models.DTO;
using TransactionsAPI.Models.Validators.Interfaces;

namespace TransactionsAPI.Models.Validators
{
    public class TransactionDataValidator : ITransactionValidator
    {
        private const string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        private readonly Regex _emailRegex;
        string[] _allowedExtensions = { ".png", ".mp3", ".tiff", ".xls", ".pdf" };

        public TransactionDataValidator()
        {
            _emailRegex = new Regex(emailPattern, RegexOptions.Compiled);
        }

        public bool Validate(TransactionDTO? row)
        {
            if (row == null) return false;

            if (!ValidateId(row.Id)) return false;

            if (!ValidateApplicationName(row.ApplicationName)) return false;

            if (!ValidateEmail(row.Email)) return false;

            if (!ValidateFileName(row.Filename)) return false;

            if (!ValidateURL(row.Url)) return false;

            if (!ValidateInception(row.Inception)) return false;

            if (!ValidateAmount(row.Amount)) return false;

            if (!ValidateAllocation(row.Allocation)) return false;

            return true;
        }

        private bool ValidateId(string id)
        {
            if (string.IsNullOrEmpty(id)) return false;

            return Guid.TryParse(id, out var result);
        }

        private bool ValidateApplicationName(string applicationName)
        {
            if (string.IsNullOrEmpty(applicationName)) return false;

            if (applicationName.Length > 200) return false;

            return true;
        }

        private bool ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;

            if (email.Length > 200) return false;

            return _emailRegex.IsMatch(email);
        }

        private bool ValidateFileName(string filename)
        {
            if (string.IsNullOrEmpty(filename)) return true;

            if (filename.Length > 300) return false;
            string fileExtension = Path.GetExtension(filename);

            return _allowedExtensions.Contains(fileExtension);
        }

        private bool ValidateURL(string url)
        {
            if (string.IsNullOrEmpty(url)) return true;
            return Uri.TryCreate(url, UriKind.Absolute, out var result);
        }

        private bool ValidateInception(string inception)
        {
            if (string.IsNullOrEmpty(inception)) return false;
            try
            {
                var result = DateTime.ParseExact(inception, "M/d/yyyy", CultureInfo.InvariantCulture);
                return result < DateTime.Now;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private bool ValidateAmount(string amountWithCurrency)
        {
            if (string.IsNullOrEmpty(amountWithCurrency)) return false;

            var currency = amountWithCurrency[0];
            if (currency != '$' && currency != '€') return false;

            var amount = amountWithCurrency.Substring(1);

            return decimal.TryParse(amount, out var result);
        }

        private bool ValidateAllocation(string allocation)
        {
            if (string.IsNullOrEmpty(allocation)) return true;
            if (!decimal.TryParse(allocation, out var result)) return false;

            return result > 0 && result < 100;
        }
    }
}
