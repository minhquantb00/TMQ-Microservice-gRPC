using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMQ.GoogleService
{
    public class TokenInfoResponse
    {
        // These six fields are included in all Google ID Tokens.
        public string iss { get; set; } // "iss": "https://accounts.google.com",
        public string sub { get; set; } // "sub": "110169484474386276334",

        public string
            azp
        { get; set; } // "azp": "1008719970978-hb24n2dstb40o45d4feuo2ukqmcc6381.apps.googleusercontent.com",

        public string
            aud
        { get; set; } // "aud": "1008719970978-hb24n2dstb40o45d4feuo2ukqmcc6381.apps.googleusercontent.com",

        public long iat { get; set; } // "iat": "1433978353",
        public long exp { get; set; } // "exp": "1433981953",


        // These seven fields are only included when the user has granted the "profile" and
        // "email" OAuth scopes to the application.
        public string email { get; set; } // "email": "testuser@gmail.com",
        public string email_verified { get; set; } // "email_verified": "true",
        public string name { get; set; } // "name" : "Test User",

        public string
            picture
        {
            get;
            set;
        } // "picture": "https://lh4.googleusercontent.com/-kYgzyAWpZzJ/ABCDEFGHI/AAAJKLMNOP/tIXL9Ir44LE/s99-c/photo.jpg",

        public string given_name { get; set; } // "given_name": "Test",
        public string family_name { get; set; } // "family_name": "User",
        public string locale { get; set; } // "locale": "en"
    }

    public record GoogleUserInfoV2
    {
        public string resourceName { get; set; }
        public string etag { get; set; }
        public Names[]? names { get; set; }
        public Genders[]? genders { get; set; }
        public Birthdays[]? birthdays { get; set; }

        public record Names
        {
            public Metadata metadata { get; set; }
            public string displayName { get; set; }
            public string familyName { get; set; }
            public string givenName { get; set; }
            public string displayNameLastFirst { get; set; }
            public string unstructuredName { get; set; }
        }

        public record Metadata
        {
            public bool primary { get; set; }
            public Source source { get; set; }
            public bool sourcePrimary { get; set; }
        }

        public record Source
        {
            public string type { get; set; }
            public string id { get; set; }
        }

        public record Genders
        {
            public Metadata1 metadata { get; set; }
            public string value { get; set; }
            public string formattedValue { get; set; }
        }

        public record Metadata1
        {
            public bool primary { get; set; }
            public Source1 source { get; set; }
        }

        public record Source1
        {
            public string type { get; set; }
            public string id { get; set; }
        }

        public record Birthdays
        {
            public Metadata2 metadata { get; set; }
            public Date? date { get; set; }
        }

        public record Metadata2
        {
            public bool primary { get; set; }
            public Source2 source { get; set; }
        }

        public record Source2
        {
            public string type { get; set; }
            public string id { get; set; }
        }

        public record Date
        {
            public int? month { get; set; }
            public int? day { get; set; }
            public int? year { get; set; }
        }
    }
}
