using System.ComponentModel.DataAnnotations;

namespace QuiosqueFood3000.Api.Helpers
{
    public static class EmailHelper
    {
        public static bool IsValidEmail(string email)
        {
            if (email == null)
            {
                return false;
            }
            else
            {
                return new EmailAddressAttribute().IsValid(email);
            }
        }
    }
}
