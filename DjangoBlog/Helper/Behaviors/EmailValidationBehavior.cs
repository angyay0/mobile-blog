using System;
using System.Net.Mail;
using Xamarin.Forms;

namespace DjangoBlog.Helper.Behaviors
{
    public class EmailValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            double result;
            bool isValid = false;
            try
            {
                MailAddress m = new MailAddress(((Entry)sender).Text);
                isValid = true;
            }
            catch
            {
                isValid = false;
            }

            ((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
        }
    }
}
