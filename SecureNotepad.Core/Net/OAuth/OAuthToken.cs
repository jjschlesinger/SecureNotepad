using System;
using System.Linq;

namespace SecureNotepad.Core.Net.OAuth
{
    [Serializable]
    public class OAuthToken
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public string AuthToken { get; set; }
        public int ExpiresIn
        {
            get
            {
                if (ExpiresOn == null)
                    return 0;

                return (int)ExpiresOn.Value.Subtract(DateTime.Now).TotalSeconds;
            }
            set
            {
                ExpiresOn = DateTime.Now.AddSeconds(value);
            }
        }

        public bool IsExpired
        {
        	get
            {
                if (ExpiresOn == null)
                    return true;

                return ExpiresOn.Value <= DateTime.Now;
            }
        }

        public DateTime? ExpiresOn { get; private set; }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = 17;
                result = result * 23 + ((AccessToken != null) ? this.AccessToken.GetHashCode() : 0);
                result = result * 23 + ((TokenType != null) ? this.TokenType.GetHashCode() : 0);
                result = result * 23 + ((AuthToken != null) ? this.AuthToken.GetHashCode() : 0);
                result = result * 23 + this.ExpiresIn.GetHashCode();
                return result;
            }
        }

        public bool Equals(OAuthToken value)
        {
            if (ReferenceEquals(null, value))
            {
                return false;
            }
            if (ReferenceEquals(this, value))
            {
                return true;
            }
            return Equals(this.AccessToken, value.AccessToken) &&
                   Equals(this.TokenType, value.TokenType) &&
                   Equals(this.AuthToken, value.AuthToken) &&
                   this.ExpiresIn == value.ExpiresIn;
        }

        public override bool Equals(object obj)
        {
            OAuthToken temp = obj as OAuthToken;
            if (temp == null)
                return false;
            return this.Equals(temp);
        }
    }
}


