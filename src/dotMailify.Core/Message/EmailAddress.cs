using System;
using System.Diagnostics;
using System.Net.Mail;

namespace dotMailify.Core.Message
{
	[DebuggerStepThrough]
    [DebuggerDisplay("{ToString()}")]
	public class EmailAddress : IEquatable<EmailAddress>
	{
		public string Address { get; }
		public string DisplayAs { get; set; }

		public EmailAddress(string address, string displayAs = null)
		{
			Address = address;
			DisplayAs = displayAs;
		}

		static public implicit operator EmailAddress(string emailAddress)
		{
			return new EmailAddress(emailAddress);
		}

        static public implicit operator MailAddress(EmailAddress emailAddress)
        {
            return new MailAddress(emailAddress.Address, emailAddress.DisplayAs);
        }

        static public implicit operator EmailAddress(MailAddress emailAddress)
        {
            return new EmailAddress(emailAddress.Address, emailAddress.DisplayName);
        }

        public override int GetHashCode()
		{
			return Address.GetHashCode();
		}

		public bool Equals(EmailAddress other)
		{
			return other != null && Address.Equals(other.Address);
		}

		public void Validate()
		{
            if(string.IsNullOrWhiteSpace(Address)) throw new ArgumentNullException(nameof(Address), Validation.EmailAddress_AddressNotSpecified);
		}

		public override string ToString()
		{
			if(!string.IsNullOrWhiteSpace(DisplayAs)) return $"{DisplayAs} <{Address}>";
			return Address;
		}

		public MailAddress ToMailAddress()
		{
			return new MailAddress(Address, DisplayAs);
		}
	}
}
